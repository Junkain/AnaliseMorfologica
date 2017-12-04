using AnaliseMorfologicaLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AnaliseMorfologica
{
    public partial class FormMain : Form
    {
        private Imagem entrada;

        public FormMain()
        {
            InitializeComponent();
        }

        private void img_Click(object sender, EventArgs e)
        {
            PictureBox pic = (sender as PictureBox);
            if (pic.SizeMode == PictureBoxSizeMode.Zoom)
            {
                pic.SizeMode = PictureBoxSizeMode.CenterImage;
            }
            else
            {
                pic.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                entrada = new Imagem(openFileDialog.FileName);

                imgEntrada.Image = entrada.CriarBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (imgSaida.Image == null)
            {
                MessageBox.Show("Nada para salvar na saída!", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                imgSaida.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            if (entrada == null)
            {
                MessageBox.Show("Nada para processar na entrada!", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Imagem saida = entrada.Clonar();

            // Processar!!!
            saida.ConverterParaEscalaDeCinza();
            int media = saida.CalcularMedia();
            saida.LimitarInvertido(media);
            saida.Erodir(3);
            //saida.ConverterParaEscalaDeCinzaMedia();

            List<Forma> formas = new List<Forma>();
            saida.CriarMapaDeFormas(formas);

            int x = 28;
            int y = 28;
            int x1 = 86;
            int y1 = 86;
            int x2 = 635;
            int y2 = 28;
            int x3 = 693;
            int y3 = 86;
            int x4 = 203;
            int y4 = 326;
            int x5 = 255;
            int y5 = 378;
            Boolean flag = false;
            int cont = 0, cont2 = 0;

            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < formas.Count; i++)
                {
                    if (formas[i].FazInterseccao(x, y, x1, y1))
                    {
                        cont++;
                    }
                }

                y += 462;
                y1 += 464;

                for (int i = 0; i < formas.Count; i++)
                {
                    if (formas[i].FazInterseccao(x2, y2, x3, y3))
                    {
                        cont2++;
                    }
                }

                y2 += 461;
                y3 += 464;

                if (cont == 3 && cont2 == 3)
                {
                    flag = true;
                }
            }

            if (!flag)
            {
                MessageBox.Show("Não é um gabarito!!");
            }
            else
            {
                int[,] questoesValidas = new int[10, 5];
                int[,] gabarito = new int[10, 5];
                cont = 0;
                cont2 = 0;

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        for (int z = 0; z < formas.Count; z++)
                        {
                            if (formas[z].FazInterseccao(x4, y4, x5, y5))
                            {
                                questoesValidas[i, j] = 1;
                            }

                            x4 += 65;
                            x5 += 65;
                        }
                    }
                    x4 = 204;
                    y4 += 64;
                    x5 = 254;
                    y5 += 65;
                }
                int cont3 = 0;
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        cont3++;
                    }
                }
                MessageBox.Show(cont.ToString());
            }

            // Exibir saída
            imgSaida.Image = saida.CriarBitmap();
        }
    }
}
