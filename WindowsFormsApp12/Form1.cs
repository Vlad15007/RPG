using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void MoveDrag(object sender)
        {
            TableLayoutPanel person = sender as TableLayoutPanel;
            
            int x = person.Location.X;
            int y = person.Location.Y;
            int zaderjka = 30;
            int silaX = 5;
            int silaY = 2;
            for(int i = 0; i < 4; i++)
            {
                person.Location = new Point(x - silaX, y + silaY);
                Thread.Sleep(zaderjka);
                person.Location = new Point(x, y);
                Thread.Sleep(zaderjka);
                person.Location = new Point(x + silaX, y - silaY);
                Thread.Sleep(zaderjka);
            }
            person.Location = new Point(x, y);
        }

        public void SetPokazatel(TableLayoutPanel table, double pokazatel, bool reverse = false)
        {
            int convert = (int)pokazatel;
            if(convert >= 100)
            {
                convert = 100;
            }
            else if(convert < 0)
            {
                convert = 0;
            }


            if (reverse == false)
            {
                table.ColumnStyles[0].Width = convert;
                table.ColumnStyles[1].Width = 100 - convert;
            }
            else
            {
                table.ColumnStyles[1].Width = convert;
                table.ColumnStyles[0].Width = 100 - convert;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Music/epic1.wav");
            player.Play();

        }

        Random random = new Random();

        #region Наш персонаж
        double myHelthMAX = 100;
        double myHelthCurrent = 100;

        double myArmorMAX = 100;
        double myArmorCurrent = 100;

        double myMannaMAX = 100;
        double myMannaCurrent = 100;

        int myOputMax = 100;
        int myOputCurrent = 100;

        int myLevel = 1;
        #endregion

        #region Противник
        double enemyHelthMAX = 100;
        double enemyHelthCurrent = 100;

        double enemyArmorMAX = 100;
        double enemyArmorCurrent = 100;

        double enemyMannaMAX = 100;
        double enemyMannaCurrent = 100;

        int chanseHeadAtack = 0;
        int chanseBodyAtack = 0;
        int chanseFootAtack = 0;

        int enemyLevel = 1;
        #endregion

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Head.Image = null;
            Body.Image = null;
            Foots.Image = null;

            PictureBox setSheald = sender as PictureBox;

            setSheald.Image = Properties.Resources.Щит2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(myLevel + 1 >= enemyLevel )
            {
                SetYron("Enemy", 20);
                MoveDrag(Enemy);
            }
            else
            {
                if(random.Next(0, 3) != 0)
                {
                    SetYron("Enemy", 20);
                    MoveDrag(Enemy);
                }
            }

            if (enemyHelthCurrent > 0)
            {
                for(int i = myLevel - 1; i < enemyLevel; i++)
                {
                    Thread.Sleep(500);
                    EnemyAtack();
                }
            }
            else
            {
                int oput = enemyLevel * random.Next(10, 15);
                AddExpiriance(oput);
                EnemyStatus.Visible = false;
                timer1.Start();
            }

            if (myHelthCurrent <= 0)
            {
                EndGame();
            }
        }

        private void AddExpiriance(int oput)
        {
            if (progressBar1.Value + oput <= progressBar1.Maximum)
            {
                progressBar1.Value += oput;
            }
            else
            {
                myLevel++;
                progressBar1.Value = 0;

                int bonus = myLevel * random.Next(20, 25);

                myHelthCurrent = myHelthMAX += bonus;
                myArmorCurrent = myArmorMAX += bonus;

                SetPokazatel(HeroArmor, myArmorCurrent);
                SetPokazatel(HeroHelth, myHelthCurrent);

                label5.Text = myLevel + "Ур.";
            }
        }


        private void EndGame()
        {
            Arena.Visible = false;
            Story.BackgroundImage = Properties.Resources.END;
            Proideno.Text = "Судьба любит смелых, но тебя сочла не достойным";
        }

        private void SetYron(string whom, int yron)
        {
            if(whom == "Hero")
            {
                if(myArmorCurrent > 0)
                {
                    myArmorCurrent -= yron;
                    double armorproc = myArmorCurrent / myArmorMAX * 100;
                    SetPokazatel(HeroArmor, armorproc);
                }
                else
                {
                    myHelthCurrent -= yron;
                    double helthpoc = myHelthCurrent / myHelthMAX * 100;
                    SetPokazatel(HeroHelth, helthpoc);
                }
            }
            else if(whom == "Enemy")
            {
                if (enemyArmorCurrent > 0)
                {
                    enemyArmorCurrent -= yron;
                    double armorproc = enemyArmorCurrent / enemyArmorMAX * 100;
                    SetPokazatel(EnemyArmor, armorproc, true);
                }
                else
                {
                    enemyHelthCurrent -= yron;
                    double helthproc = enemyHelthCurrent / enemyHelthMAX * 100;
                    SetPokazatel(EnemyHelth, helthproc, true);
                }
            }
        }

        private void EnemyAtack()
        {
            int atack = random.Next(0, 100);
            int yron = 0;
            PictureBox atackObject = null;

            if(atack <= chanseHeadAtack)
            {
                atackObject = Head;
                yron = 25;
            }
            else if (atack <= chanseBodyAtack)
            {
                atackObject = Body;
                yron = 20;
            }
            else
            {
                atackObject = Foots;
                yron = 15;
            }

            if (atackObject.Image == null) atackObject.BackColor = Color.Brown;
            else
            {
                yron = 5;
                atackObject.BackColor = Color.Teal;
            }
            
            atackObject.Update();

            SetYron("Hero", yron);
            MoveDrag(HeroPerson);

            atackObject.BackColor = Color.Transparent;
            atackObject.Update();
        }


        int rasstoianie = 0;
        string[] fightersName = {"Спартанец", "Рыцарь" };
        int[,] fightersData =
        {
            {100, 100, 20 },
            {80, 150, 15 }
        };
        int[,] fightersAtack =
        {
            {20, 80},
            {40, 50}
        };

        private void GenerateEnemy()
        {
            int fighter = random.Next(0, fightersName.Length);
            int level = random.Next(myLevel, myLevel + 5);
            int levelPlus = (myLevel- level) * 20;

            enemyHelthCurrent = enemyHelthMAX = fightersData[fighter, 0] + levelPlus;
            enemyArmorCurrent = enemyArmorMAX = fightersData[fighter, 1] + levelPlus;

            chanseHeadAtack = fightersAtack[fighter, 0];
            chanseBodyAtack = fightersAtack[fighter, 1];

            double helthproc = enemyHelthCurrent / enemyHelthMAX * 100;
            double armorproc = enemyArmorCurrent / enemyArmorMAX * 100;
            SetPokazatel(EnemyHelth, helthproc, true);
            SetPokazatel(EnemyArmor, armorproc, true);

            Proideno.Text = rasstoianie + "м - Вы столкнулись с " + fightersName[fighter];
            label1.Text = level + "Ур.";
            enemyLevel = level;

            if (fighter == 0)
            {
                EnemyHead.BackgroundImage = Properties.Resources.Спартанец_голова;
                EnemyBody.BackgroundImage = Properties.Resources.Спартанец_тело2;
                EnemyFoots.BackgroundImage = Properties.Resources.Спартанец_ноги;
            }
            else if(fighter == 1)
            {
                EnemyHead.BackgroundImage = Properties.Resources.РыцарьГолова;
                EnemyBody.BackgroundImage = Properties.Resources.РыцарьТело;
                EnemyFoots.BackgroundImage = Properties.Resources.РыцарьНоги;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            Proideno.Text = rasstoianie + "м";
            if(random.Next(0, 100) <= 1)
            {
                timer1.Stop();
                GenerateEnemy();
                EnemyStatus.Visible = true;
            }    
            
            if(myArmorCurrent < 100)
            {
                myArmorCurrent++;
                SetPokazatel(HeroArmor, myArmorCurrent);
            }
            if (myMannaCurrent < 100)
            {
                myMannaCurrent++;
            }

            if(rasstoianie % 10 == 0)
            {
                AddExpiriance(2);
            }
            rasstoianie++;
        }
    }
}
