using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Felhasznalo
{
    public partial class Form1 : Form
    {
        MySqlConnection conn = null;
        MySqlCommand cmd = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "felhasznalo";
            conn = new MySqlConnection(builder.ConnectionString);
            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message + Environment.NewLine + "A program leáll");

                Environment.Exit(0);
            }
            finally
            {
                conn.Close();
            }
            listBox1_update();
        }

        private void listBox1_update()
        {
            conn.Close();
            listBox1.Items.Clear();
            cmd.CommandText = "SELECT * FROM `felh`;";
            conn.Open();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    FELHASZNALO uj = new FELHASZNALO(dr.GetInt32("id"), dr.GetString("name"), dr.GetDateTime("Szuldat"), dr.GetString("kep"));
                    listBox1.Items.Add(uj);
                }
            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Close();
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Adjon meg nevet");
                textBox1.Focus();
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Adjon meg kep nevet");
                textBox2.Focus();
                
                return;
            }
            cmd.CommandText = "INSERT INTO `felh`(`name`, `Szuldat` ,`kep`) VALUES (@nev ,@Szuldat,@kep)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Szuldat",dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@nev", textBox1.Text);            
            cmd.Parameters.AddWithValue("@kep", openFileDialog1.SafeFileName);
         
            conn.Open();
            try
            {
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Sikeresen rögzítve!");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    listBox1_update();
                }
                else
                {
                    MessageBox.Show("sikertelen rögzítés!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           


            openFileDialog1.Filter = "*.png|*.png|*.jpg|*.jpg|*.jfif|*.jfif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string kepFajl = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(kepFajl);
                textBox2.Text = openFileDialog1.SafeFileName;

            }
        }

        public void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                return;
            }
            FELHASZNALO felhasznalo = (FELHASZNALO)listBox1.SelectedItem;
            textBox1.Text = felhasznalo.Nev;
            textBox2.Text = felhasznalo.Kep;
            textBox3.Text = felhasznalo.Id.ToString();
            dateTimePicker1.Value = felhasznalo.Szuldat1;
           

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("nincs adat");
                return;
            }
            cmd.CommandText = "DELETE FROM `felh` WHERE id = @id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@id", textBox3.Text);
            cmd.Parameters.AddWithValue("@nev", textBox1.Text);
            cmd.Parameters.AddWithValue("@kep", textBox2.Text);
            cmd.Parameters.AddWithValue("@Szuldat", dateTimePicker1.Value);


            conn.Open();
            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Sikeresen törlés!");
                conn.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                listBox1_update();


            }
            else
            {
                MessageBox.Show("sikertelen  törlés!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Nincs kijelölve adat!");
                return;
            }
            cmd.Parameters.Clear();
            FELHASZNALO felhasznalo = (FELHASZNALO)listBox1.SelectedItem;
            cmd.CommandText = "UPDATE `felh` SET `name`= @nev,`Szuldat`= @Szuldat,`kep`= @kep WHERE `id` = @id";
            cmd.Parameters.AddWithValue("@id", textBox3.Text);
            cmd.Parameters.AddWithValue("@nev", textBox1.Text);
            cmd.Parameters.AddWithValue("@kep", textBox2.Text);
            cmd.Parameters.AddWithValue("@Szuldat", dateTimePicker1.Value);

            conn.Open();
            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Módosítás sikeres votl!");
                conn.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";

                listBox1_update();

            }
            else
            {
                MessageBox.Show("Az adatok módosítása sikertelen!");
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }  
    
    
}
