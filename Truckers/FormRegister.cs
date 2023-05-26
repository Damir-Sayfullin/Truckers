﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server;

namespace Truckers
{
    public partial class FormRegister : Form
    {
        bool password_show = false;
        private FormLogin formLogin;
        public FormRegister(FormLogin formLogin)
        {
            InitializeComponent();
            this.formLogin = formLogin;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            formLogin.Show();
            this.Close();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.Text == "")
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
            else
            {
                try
                {
                    IRemoteObject remoteObject = (IRemoteObject)Activator.GetObject(
                     typeof(IRemoteObject),
                     "tcp://localhost:8080/RemoteObject.rem");
                    string result = remoteObject.Registration(textBox1.Text, textBox2.Text, textBox3.Text, comboBox1.Text);

                    if (result == "0")
                    {
                        this.TopMost = true;
                        MessageBox.Show(
                                "Регистрация прошла успешно!",
                                "Успех!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                        this.TopMost = false;
                    }
                    else if (result == "1")
                    {
                        this.TopMost = true;
                        MessageBox.Show(
                                "Пользователь с таким логином уже существует!",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                        this.TopMost = false;
                    }
                }
                catch
                {
                    this.TopMost = true;
                    MessageBox.Show(
                            "Не удалось подключиться к серверу!",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                    this.TopMost = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            password_show = !password_show;
            if (password_show)
            {
                button2.BackgroundImage = Properties.Resources.EyeClosed;
                textBox3.UseSystemPasswordChar = false;
            }
            else
            {
                button2.BackgroundImage = Properties.Resources.EyeOpened;
                textBox3.UseSystemPasswordChar = true;
            }
        }
    }
}
