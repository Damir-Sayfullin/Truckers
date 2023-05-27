﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server;

namespace Truckers
{
    public partial class FormLogist : Form
    {
        RemoteObjectTCP remoteTCP; // удаленный объект
        FormLogin formLogin; // ссылка на форму авторизации
        DataTable cargoDataTable; // таблица с грузами
        public FormLogist(FormLogin formLogin, string username)
        {
            InitializeComponent();
            this.formLogin = formLogin; // присваивание ссылки на форму авторизации
            label1.Text = "Логист: " + username;
            ConnectToServer();
            CargoReload();
        }

        private void ConnectToServer() // установка соединения с сервером
        {
            RemotingConfiguration.Configure("C:/My Files/Универ/3 курс/Технологии программирования/TP_Truckers/Truckers/ClientConfig.config", false);
            remoteTCP = new RemoteObjectTCP();
            ILease lease = (ILease)remoteTCP.InitializeLifetimeService();
            ClientSponsor clientTCPSponsor = new ClientSponsor();
            lease.Register(clientTCPSponsor);
        }

        private void CargoReload() // обновление таблицы
        {
            // получение данных 
            cargoDataTable = remoteTCP.Logist_CargoReload();
            // очистка выпадающего списка
            comboBox_ID.Items.Clear();

            // проходим по каждой строке в cargoDataTable
            foreach (DataRow row in cargoDataTable.Rows)
            {
                // получение значения поля ID из текущей строки
                object value = row["ID"];

                // добавление значения в comboBox_ID
                comboBox_ID.Items.Add(value);
            }
        }
        
        private void buttonReload_Click(object sender, EventArgs e) // при нажатии на кнопку "Обновить"
        {
            // обновление таблицы
            CargoReload();
        }

        private void comboBox_ID_SelectedValueChanged(object sender, EventArgs e) // при выборе ID в выпадающем списке
        {
            // поиск строк с выбранным ID
            DataRow[] IDRow = cargoDataTable.Select(String.Format("ID = '{0}'", comboBox_ID.SelectedItem.ToString()));
            foreach (DataRow row in IDRow)
            {
                // изменение полей в соответсвии с выбраным ID
                textBoxDriverID.Text = row["DriverID"].ToString();
                comboBoxStatus.Text = row["Status"].ToString();
                textBoxCargo.Text = row["Cargo"].ToString();
                textBoxWeight.Text = row["Weight"].ToString();
                textBoxFrom.Text = row["From"].ToString();
                textBoxTo.Text = row["To"].ToString();
            }

        }

        private void buttonExit_Click(object sender, EventArgs e) // при нажатии на кнопку "Выйти из системы"
        {
            // открытие формы авторизации
            formLogin.Show();
            // закрытие этой формы регистрации
            this.Close();
            // освобождение памяти
            this.Dispose();
        }

        private void buttonSave_Click(object sender, EventArgs e) // при нажатии на кнопку "Сохранить"
        {
            // проверка на пустые поля
            if (comboBox_ID.Text == "" || textBoxDriverID.Text == "" || comboBoxStatus.Text == "" || textBoxCargo.Text == "" || textBoxWeight.Text == "" || textBoxFrom.Text == "" || textBoxTo.Text == "")
            {
                this.TopMost = true;
                MessageBox.Show(
                        "Не все поля заполнены!",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                this.TopMost = false;
            }
            // если все поля заполнены
            else
            {
                // вызов метода Logist_CargoSave у удаленного объекта
                string result = remoteTCP.Logist_CargoSave(comboBox_ID.Text, textBoxDriverID.Text, comboBoxStatus.Text, textBoxCargo.Text, textBoxWeight.Text, textBoxFrom.Text, textBoxTo.Text);
                // если изменение прошло успешно
                if (result == "0")
                {
                    this.TopMost = true;
                    MessageBox.Show(
                            "Данные успешно сохранены!",
                            "Успех!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                    this.TopMost = false;
                }
                // обновление таблицы
                CargoReload();
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e) // при нажатии на кнопку "Добавить"
        {
            // проверка на пустые поля
            if (textBoxDriverID.Text == "" || comboBoxStatus.Text == "" || textBoxCargo.Text == "" || textBoxWeight.Text == "" || textBoxFrom.Text == "" || textBoxTo.Text == "")
            {
                this.TopMost = true;
                MessageBox.Show(
                        "Не все поля заполнены!",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                this.TopMost = false;
            }
            // если все поля заполнены
            else
            {
                // вызов метода Logist_CargoAdd у удаленного объекта
                string result = remoteTCP.Logist_CargoAdd(textBoxDriverID.Text, comboBoxStatus.Text, textBoxCargo.Text, textBoxWeight.Text, textBoxFrom.Text, textBoxTo.Text);
                // если изменение прошло успешно
                if (result == "0")
                {
                    this.TopMost = true;
                    MessageBox.Show(
                            "Данные успешно добавлены!",
                            "Успех!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                    this.TopMost = false;
                }
                // обновление таблицы
                CargoReload();
            }
        }
    }
}
