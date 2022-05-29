using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using LiveCharts;
using LiveCharts.Wpf;

namespace _213.Forms
{
    public partial class FormPersonal : Form
    {
        static string index_selected_rows;
        static string id_selected_rows;

        public FormPersonal()
        {
            InitializeComponent();
        }
        public class DBOperation
        {
            //Переменная соединения

            MySqlConnection conn = new MySqlConnection("server=chuc.caseum.ru;port=33333;user=st_1_18_29;database=is_1_18_st29_VKR;password=45394869;");


            DataTable dt = new DataTable();
            BindingSource bs = new BindingSource();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            private MySqlDataAdapter MyDA = new MySqlDataAdapter();
            //Объявление BindingSource, основная его задача, это обеспечить унифицированный доступ к источнику данных.
            private BindingSource bSource = new BindingSource();
            private DataSet ds = new DataSet();
            //Представляет одну таблицу данных в памяти.
            private DataTable table = new DataTable();



            public void GetListStaff(BindingSource bs1, DataGridView dg1)
            {

                //Запрос для вывода строк в БД
                string commandStr = "SELECT id AS 'ID',fio AS 'ФИО', age AS 'Средний возраст',exp AS 'Средний опыт работы',dolzh AS 'Должность'  FROM personal";
                //Открываем соединение
                conn.Open();
                //Объявляем команду, которая выполнить запрос в соединении conn
                MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
                //Заполняем таблицу записями из БД
                MyDA.Fill(table);
                //Указываем, что источником данных в bindingsource является заполненная выше таблица
                bs1.DataSource = table;
                //Указываем, что источником данных ДатаГрида является bindingsource
                dg1.DataSource = bs1;
                //Закрываем соединение
                conn.Close();
            }
            public void InsertStaff(TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, TextBox textBox5)
            {
                string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_18_29;database=is_1_18_st29_VKR;password=45394869;";

                MySqlConnection conn = new MySqlConnection(connStr);
                //Получение новых параметров пользователя
                string new_id = textBox1.Text;
                string new_age = textBox3.Text;
                string new_exp = textBox4.Text;
                string new_dolzh = textBox5.Text;
                string new_fio = textBox2.Text;

                if (textBox1.Text.Length > 0)
                {
                    //Формируем строку запроса на добавление строк
                    string sql_insert_clothes = " INSERT INTO `personal` (id, age,exp,dolzh,fio) " +
                        "VALUES ('" + new_id + "', '" + new_age + "', '" + new_exp + "', '" + new_dolzh + "','"+ new_fio+"')";


                    //Посылаем запрос на добавление данных
                    MySqlCommand insert_clothes = new MySqlCommand(sql_insert_clothes, conn);
                    try
                    {
                        conn.Open();
                        insert_clothes.ExecuteNonQuery();
                        MessageBox.Show("Добавление сотрудника прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка добавления сотрудника \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка!", "Информация");
                }
            }
            public void reload_list(BindingSource bs1, DataGridView dg1)
            {
                //Чистим виртуальную таблицу
                table.Clear();
                //Вызываем метод получения записей, который вновь заполнит таблицу
                DBOperation DBO = new DBOperation();
                DBO.GetListStaff(bs1, dg1);
            }
            public void GetCurrentID(DataGridView dataGridView1)
            {
                index_selected_rows = dataGridView1.SelectedCells[0].RowIndex.ToString();
                // MessageBox.Show("Индекс выбранной строки" + index_selected_rows);
                id_selected_rows = dataGridView1.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
                // MessageBox.Show("Содержимое поля Код, в выбранной строке" + id_selected_rows);
                class_edit_user.id = id_selected_rows;
            }
            public void DeleteStaff(int id)
            {
                string del = "DELETE FROM personal WHERE id = " + id;
                MySqlCommand del_stats = new MySqlCommand(del, conn);

                try
                {
                    conn.Open();
                    del_stats.ExecuteNonQuery();
                    //this.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления пользователя \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }

            }
            public void UpdateStaff(TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, TextBox textBox5)
            {
                //Получаем ID пользователя
                string id = class_edit_user.id;
                string SQL_izm = "UPDATE personal SET id=N'" + textBox1.Text + "', fio=N'" + textBox2.Text + "', age=N'" + textBox3.Text + "'," +
                    " exp=N'" + textBox4.Text + "', dolzh=N'" + textBox5.Text + "' where id=" + id;
                MessageBox.Show(SQL_izm);
                MySqlConnection conn = new MySqlConnection("server=chuc.caseum.ru;port=33333;user=st_1_18_29;database=is_1_18_st29_VKR;password=45394869;");
                conn.Open();
                MySqlCommand command1 = new MySqlCommand(SQL_izm, conn);
                MySqlDataReader dr = command1.ExecuteReader();
                dr.Close();
                conn.Close();
                MessageBox.Show("Данные изменены");
                //this.Activate();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
            }
        }

        private void FormPersonal_Load(object sender, EventArgs e)
        {
            DBOperation DBO = new DBOperation();
            DBO.GetListStaff(bindingSource1, dataGridView1);

            fillChart();
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;

            label1.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label5.Visible = false;
            label2.Visible = false;

            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;

            //Ширина полей
            dataGridView1.Columns[0].FillWeight = 10;
            dataGridView1.Columns[1].FillWeight = 10;
            dataGridView1.Columns[2].FillWeight = 10;
            dataGridView1.Columns[3].FillWeight = 10;
            dataGridView1.Columns[4].FillWeight = 10;

            //Растягивание полей грида
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //Убираем заголовки строк
            dataGridView1.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView1.ColumnHeadersVisible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        private void fillChart()
        {
            MySqlConnection conn = new MySqlConnection("server=chuc.caseum.ru;port=33333;user=st_1_18_29;database=is_1_18_st29_VKR;password=45394869;");
            DataSet ds = new DataSet();
            conn.Open();
            MySqlDataAdapter adapt = new MySqlDataAdapter("SELECT exp,fio FROM personal", conn);
            adapt.Fill(ds);
            chart1.DataSource = ds;
            //set the member of the chart data source used to data bind to the X-values of the series
            chart1.Series["Sales"].XValueMember = "fio";
            //set the member columns of the chart data source used to data bind to the X-values of the series
            chart1.Series["Sales"].YValueMembers = "exp";
            chart1.Titles.Add("Опыт работы");
            chart1.Update();
            conn.Close();

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string fromDGtoTB = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox1.Text =
                dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox2.Text =
                dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox3.Text =
               dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox4.Text =
              dataGridView1[4, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox5.Text =
              dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DBOperation DBO = new DBOperation();
            DBO.InsertStaff(textBox1, textBox2, textBox3, textBox4,textBox5);
            DBO.reload_list(bindingSource1, dataGridView1);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DBOperation DBO = new DBOperation();
            DBO.GetCurrentID(dataGridView1);
            DBO.UpdateStaff(textBox1, textBox2, textBox3, textBox4,textBox5);
            DBO.reload_list(bindingSource1, dataGridView1);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DBOperation DBO = new DBOperation();
            DBO.GetCurrentID(dataGridView1);
            DBO.DeleteStaff(Convert.ToInt32(id_selected_rows));
            DBO.reload_list(bindingSource1, dataGridView1);
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (toolStripTextBox1.Text != "")
            {
                bindingSource1.Filter = "ФИО='" + toolStripTextBox1.Text + "'";
            }
            else
            {
                bindingSource1.RemoveFilter();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            label1.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label5.Visible = true;
            label2.Visible = true;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
