using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;




namespace GozlukcuTakipOtomasyonu
{
    public partial class Form1 : Form
    {

        string eminbayiri = "Data Source=gozlukcu.sqlite;Version=3;";
        public Form1()
        {
            InitializeComponent();
        }




        private void MusterileriListele()
        {
            listView1.Items.Clear();

            using(SQLiteConnection tech = new SQLiteConnection(eminbayiri))
            {
                string query = "SELECT * FROM Musteriler";
                SQLiteCommand komut = new SQLiteCommand(query, tech);
                tech.Open();

                SQLiteDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    ListViewItem item = new ListViewItem(oku["MusteriID"].ToString());
                    item.SubItems.Add(oku["GozNumara"].ToString());
                    item.SubItems.Add(oku["Ad"].ToString());
                    item.SubItems.Add(oku["Soyad"].ToString());
                    item.SubItems.Add(oku["TC"].ToString());
                    item.SubItems.Add(oku["Telefon"].ToString());
                    item.SubItems.Add(oku["Email"].ToString());
                    item.SubItems.Add(oku["Adres"].ToString());

                    listView1.Items.Add(item);
                }
                tech.Close();
                
                
                
            }
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection tech = new SQLiteConnection(eminbayiri))
                {
                    tech.Open();
                    string query = "INSERT INTO Musteriler (GozNumara, Ad, Soyad, TC, Telefon, Email, Adres) " +
                                   "VALUES (@GozNumara, @Ad, @Soyad, @TC, @Telefon, @Email, @Adres)";
                    SQLiteCommand komut = new SQLiteCommand(query, tech);
                    komut.Parameters.AddWithValue("@GozNumara", textBox1.Text);
                    komut.Parameters.AddWithValue("@Ad", textBox2.Text);
                    komut.Parameters.AddWithValue("@Soyad", textBox3.Text);
                    komut.Parameters.AddWithValue("@TC", textBox4.Text);
                    komut.Parameters.AddWithValue("@Telefon", textBox5.Text);
                    komut.Parameters.AddWithValue("@Email", textBox6.Text);
                    komut.Parameters.AddWithValue("@Adres", textBox7.Text);


                    komut.ExecuteNonQuery();
                    MessageBox.Show("Müşteri başarıyla eklendi.");
                    MusterileriListele();
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Genel Hata: " + ex.Message);
            }
        }


        private void StokListele()
        {
            listView2.Items.Clear();

            using (SQLiteConnection tech = new SQLiteConnection(eminbayiri))
            {
                string query = "SELECT * FROM Stok";
                SQLiteCommand cmd = new SQLiteCommand(query, tech);
                tech.Open();

                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["GozlukID"].ToString());
                    item.SubItems.Add(reader["GozlukTipi"].ToString());
                    item.SubItems.Add(reader["Marka"].ToString());
                    item.SubItems.Add(reader["CerceveRengi"].ToString());
                    item.SubItems.Add(reader["StokSayisi"].ToString());

                    listView2.Items.Add(item);
                }

                tech.Close();
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            
        
            int stokSayisi;

            if (!int.TryParse(textBox11.Text, out stokSayisi))
            {
                MessageBox.Show("Lütfen geçerli bir stok sayısı girin!");
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(eminbayiri))
                {
                    string query = "INSERT INTO Stok (GozlukTipi, Marka, CerceveRengi, StokSayisi) " +
                                   "VALUES (@GozlukTipi, @Marka, @CerceveRengi, @StokSayisi)";

                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@GozlukTipi", textBox8.Text);
                    cmd.Parameters.AddWithValue("@Marka", textBox9.Text);
                    cmd.Parameters.AddWithValue("@CerceveRengi", textBox10.Text);
                    cmd.Parameters.AddWithValue("@StokSayisi", stokSayisi);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Stok başarıyla eklendi.");
                    StokListele();
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("SQLite Hatası: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Genel Hata: " + ex.Message);
            }
        

    }

        private void button3_Click(object sender, EventArgs e)
        {
            int talep, satilan;
            double fiyat;

            if (!int.TryParse(textBox13.Text, out talep) ||
                !double.TryParse(textBox14.Text, out fiyat) ||
                !int.TryParse(textBox15.Text, out satilan))
            {
                MessageBox.Show("Lütfen geçerli sayısal değerler girin.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (SQLiteConnection tech = new SQLiteConnection(eminbayiri))
                {
                    string query = "INSERT INTO Satislar (GozlukCesidi, TalepSayisi, Fiyat, SatilanAdet) " +
                                   "VALUES (@GozlukCesidi, @TalepSayisi, @Fiyat, @SatilanAdet)";
                    SQLiteCommand komut = new SQLiteCommand(query, tech);
                    komut.Parameters.AddWithValue("@GozlukCesidi", textBox12.Text);
                    komut.Parameters.AddWithValue("@TalepSayisi", talep);
                    komut.Parameters.AddWithValue("@Fiyat", fiyat);
                    komut.Parameters.AddWithValue("@SatilanAdet", satilan);

                    tech.Open();
                    komut.ExecuteNonQuery();
                    tech.Close();

                    MessageBox.Show("Satış başarıyla kaydedildi.");
                    SatisListele();
                }
            }
            catch (SQLiteException err)
            {
                MessageBox.Show("Veritabanı Hatası: " + err.Message);
            }
            catch (Exception err)
            {
                MessageBox.Show("Genel Hata: " + err.Message);
            }
        }

        private void SatisListele()
        {
            listView3.Items.Clear();

            using (SQLiteConnection tech = new SQLiteConnection(eminbayiri))
            {
                string query = "SELECT * FROM Satislar";
                SQLiteCommand komut = new SQLiteCommand(query, tech);
                tech.Open();

                SQLiteDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    ListViewItem item = new ListViewItem(oku["SatisID"].ToString());
                    item.SubItems.Add(oku["GozlukCesidi"].ToString());
                    item.SubItems.Add(oku["TalepSayisi"].ToString());
                    item.SubItems.Add(oku["Fiyat"].ToString());
                    item.SubItems.Add(oku["SatilanAdet"].ToString());

                    listView3.Items.Add(item);
                }

                tech.Close();
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            SatisListele();
            StokListele();
            MusterileriListele();

            listView1.View = View.Details;
            listView1.Columns.Add("ID", 40);
            listView1.Columns.Add("Göz No", 70);
            listView1.Columns.Add("Ad", 100);
            listView1.Columns.Add("Soyad", 100);
            listView1.Columns.Add("TC", 90);
            listView1.Columns.Add("Telefon", 100);
            listView1.Columns.Add("Email", 120);
            listView1.Columns.Add("Adres", 150);


            listView2.View = View.Details;
            listView2.Columns.Add("ID", 40);
            listView2.Columns.Add("Gözlük Tipi", 100);
            listView2.Columns.Add("Marka", 100);
            listView2.Columns.Add("Çerçeve Rengi", 100);
            listView2.Columns.Add("Stok Sayısı", 80);

            listView3.View = View.Details;
            listView3.Columns.Add("ID", 40);
            listView3.Columns.Add("GözlÜk Çeşidi", 100);
            listView3.Columns.Add("Talep Sayısı", 100);
            listView3.Columns.Add("Ürünün Fiyatı", 100);
            listView3.Columns.Add("Satılan Adet", 80);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        
            if (listView3.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir kayıt seçin.");
                return;
            }

            string satisID = listView3.SelectedItems[0].SubItems[0].Text;

            DialogResult result = MessageBox.Show("Bu satışı silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(eminbayiri))
                    {
                        string query = "DELETE FROM Satislar WHERE SatisID = @id";
                        SQLiteCommand komut = new SQLiteCommand(query, conn);
                        komut.Parameters.AddWithValue("@id", satisID);

                        conn.Open();
                        komut.ExecuteNonQuery();
                        conn.Close();
                    }

                    MessageBox.Show("Satış başarıyla silindi.");
                    SatisListele();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            
        }

    }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir kayıt seçin.");
                return;
            }

            string gozlukID = listView2.SelectedItems[0].SubItems[0].Text;

            DialogResult result = MessageBox.Show("Bu satışı silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SQLiteConnection tech = new SQLiteConnection(eminbayiri))
                    {
                        string query = "DELETE FROM Stok WHERE GozlukID = @id";
                        SQLiteCommand komut = new SQLiteCommand(query, tech);
                        komut.Parameters.AddWithValue("@id", gozlukID);

                        tech.Open();
                        komut.ExecuteNonQuery();
                        tech.Close();
                    }

                    MessageBox.Show("Stok başarıyla silindi.");
                    StokListele();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir kayıt seçin.");
                return;
            }

            string musteriID = listView1.SelectedItems[0].SubItems[0].Text;

            DialogResult result = MessageBox.Show("Bu satışı silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SQLiteConnection tech = new SQLiteConnection(eminbayiri))
                    {
                        string query = "DELETE FROM Musteriler WHERE MusteriID = @id";
                        SQLiteCommand komut = new SQLiteCommand(query, tech);
                        komut.Parameters.AddWithValue("@id", musteriID);

                        tech.Open();
                        komut.ExecuteNonQuery();
                        tech.Close();
                    }

                    MessageBox.Show("Stok başarıyla silindi.");
                    MusterileriListele();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }

            }
        }
    }
}
