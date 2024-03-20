﻿using AntdUI;
using OllamaSharp.Models;
using OllamaSharp.Streamer;
using Onllama.Tiny.Properties;

namespace Onllama.Tiny
{
    public partial class FormImport : Form
    {
        public FormImport()
        {
            InitializeComponent();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            var f = new OpenFileDialog();
            if (f.ShowDialog() == DialogResult.OK) input1.Text = f.FileName;
            inputName.Text = f.SafeFileNames.Last().Split('.', '-').First();
            foreach (var item in select1.Items)
                if (input1.Text.Contains(item.ToString()))
                    select1.SelectedValue = item.ToString();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            AntdUI.Modal.open(new AntdUI.Modal.Config(this, "您确定要导入模型吗？",
                new[]
                {
                    new Modal.TextLine(inputName.Text, Style.Db.Primary)
                }, TType.Success)
            {
                OkType = TTypeMini.Success,
                OkText = "导入",
                OnOk = _ =>
                {
                    try
                    {
                        Task.Run(() => Form1.OllamaApi.CreateModel(
                            new CreateModelRequest
                                {ModelFileContent = inputMf.Text, Name = inputName.Text, Stream = true},
                            new ActionResponseStreamer<CreateStatus>(x => Invoke(() => Text = x.Status)))).Wait();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }

                    Invoke(Close);
                    return true;
                }
            });
        }

        private void select1_SelectedValueChanged(object sender, object value)
        {
            inputMf.Text = "FROM " + input1.Text + Environment.NewLine;
            if (value.ToString() == "qwen") inputMf.Text += Resources.qwenTmp;
            if (value.ToString() == "yi") inputMf.Text += Resources.yiTmp;
            if (value.ToString() == "gemma") inputMf.Text += Resources.gemmaTmp;
            if (value.ToString() == "mistral") inputMf.Text += Resources.mistralTmp;
        }

        private void FormImport_Load(object sender, EventArgs e)
        {

        }
    }
}