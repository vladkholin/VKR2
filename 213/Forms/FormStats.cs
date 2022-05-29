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
using System.Windows.Forms.DataVisualization.Charting;
using LiveCharts.Charts;



namespace _213.Forms
{
    public partial class FormStats : Form
    {
        static string index_selected_rows;
        static string id_selected_rows;
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
                string commandStr = "SELECT id AS 'ID', type AS 'Тип', count AS 'Количество' FROM statistic";
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
            public void InsertStaff(TextBox textBox1, TextBox textBox2, TextBox textBox3)
            {
                string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_18_29;database=is_1_18_st29_VKR;password=45394869;";

                MySqlConnection conn = new MySqlConnection(connStr);
                //Получение новых параметров пользователя
                string new_id = textBox1.Text;
                string new_type = textBox2.Text;
                
                string new_count = textBox3.Text;

                if (textBox1.Text.Length > 0)
                {
                    //Формируем строку запроса на добавление строк
                    string sql_insert_clothes = " INSERT INTO `statistic` (id, type,count) " +
                        "VALUES ('" + new_id + "', '" + new_type + "', '" + new_count + "')";


                    //Посылаем запрос на добавление данных
                    MySqlCommand insert_clothes = new MySqlCommand(sql_insert_clothes, conn);
                    try
                    {
                        conn.Open();
                        insert_clothes.ExecuteNonQuery();
                        MessageBox.Show("Добавление записи прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка добавления записи \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string del = "DELETE FROM statistic WHERE id = " + id;
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
            public void UpdateStaff(TextBox textBox1, TextBox textBox2,  TextBox textBox3)
            {
                //Получаем ID пользователя
                string id = class_edit_user.id;
                string SQL_izm = "UPDATE statistic SET id=N'" + textBox1.Text + "', type=N'" + textBox2.Text + "'," +
                    " count=N'" + textBox3.Text + "' where id=" + id;
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
                
            }
        }
        public FormStats()
        {
            InitializeComponent();
            

        }
        private void fillChart()
        {
            MySqlConnection conn = new MySqlConnection("server=chuc.caseum.ru;port=33333;user=st_1_18_29;database=is_1_18_st29_VKR;password=45394869;");
            DataSet ds = new DataSet();
            conn.Open();
            MySqlDataAdapter adapt = new MySqlDataAdapter("SELECT type,count FROM statistic", conn);
            adapt.Fill(ds);
            chart1.DataSource = ds;
            //set the member of the chart data source used to data bind to the X-values of the series
            chart1.Series["Sales"].XValueMember = "type";
            //set the member columns of the chart data source used to data bind to the X-values of the series
            chart1.Series["Sales"].YValueMembers = "count";
            chart1.Titles.Add("Site sale");
            chart1.Update();
            conn.Close();
            
        }
        

        public void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void FormStats_Load(object sender, EventArgs e)
        {
            fillChart();
            chart1.Update();
            DBOperation DBO = new DBOperation();
            DBO.GetListStaff(bindingSource1, dataGridView1);
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            

            label1.Visible = false;
            label2.Visible = false;
            label4.Visible = false;

            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;

            //Ширина полей
            dataGridView1.Columns[0].FillWeight = 10;
            dataGridView1.Columns[1].FillWeight = 10;
            dataGridView1.Columns[2].FillWeight = 10;
            
            

            //Растягивание полей грида
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            
        

            //Убираем заголовки строк
            dataGridView1.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView1.ColumnHeadersVisible = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            chart1.Series["Sales"].ChartType = SeriesChartType.Bar;


        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {
          
        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void elementHost1_ChildChanged_1(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            chart1.Series["Sales"].ChartType = SeriesChartType.Area;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            chart1.Series["Sales"].ChartType = SeriesChartType.Line;
        }

        private void button3_Click(object sender, EventArgs e)
        {
          

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
           
        }

       

        private void chart1_Click_3(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string fromDGtoTB = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox1.Text =
                dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox2.Text =
                dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            
            textBox3.Text =
               dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
        }

        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
            
        }

        private void pieChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
         
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            DBOperation DBO = new DBOperation();
            DBO.InsertStaff(textBox1, textBox2, textBox3);
            DBO.reload_list(bindingSource1, dataGridView1);
        }

        private void toolStripButton5_Click_2(object sender, EventArgs e)
        {
            DBOperation DBO = new DBOperation();
            DBO.GetCurrentID(dataGridView1);
            DBO.UpdateStaff(textBox1, textBox2, textBox3);
            DBO.reload_list(bindingSource1, dataGridView1);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            DBOperation DBO = new DBOperation();
            DBO.GetCurrentID(dataGridView1);
            DBO.DeleteStaff(Convert.ToInt32(id_selected_rows));
            DBO.reload_list(bindingSource1, dataGridView1);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;


            label1.Visible = true;
            label2.Visible = true;
            label4.Visible = true;
        }
    }
}
