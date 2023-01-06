using System;
using System.Collections.Generic;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using mrousavy;
using WindowsInput.Native;
using WindowsInput;

namespace Geeker
{
    public partial class MainWindow : Window
    {
        List<string[]> Binds = new List<string[]>();
        List<HotKey> keys = new List<HotKey>();
        InputSimulator sim = new InputSimulator();

        public MainWindow()
        {

            InitializeComponent();

        }
        
        private void AddDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (TranslationTextbox.Text != string.Empty && (CtrlDropdown.SelectedItem != null || ShiftDropdown.SelectedItem != null || AltDropdown.SelectedItem != null) && LetterDropdown.SelectedItem != null && (CtrlDropdown.SelectedIndex != 0 || ShiftDropdown.SelectedIndex != 0 || AltDropdown.SelectedIndex != 0))
            {
                var row = new RowDefinition();
                row.Height = GridLength.Auto;
                GeekerGrid.RowDefinitions.Add(row);

                var row1 = new RowDefinition();
                row1.Height = GridLength.Auto;
                GeekerGrid.RowDefinitions.Add(row1);

                //define base textblocks
                var Translation = new TextBlock();
                var Bind = new TextBlock();

                string[] hotkey = getHotkey(TranslationTextbox.Text);
                for (int i = 0; i < 3; i++)
                {
                    if (hotkey[i] != "None")
                    {
                        Bind.Text += hotkey[i] + " + ";
                    }
                }
                if (hotkey[3] != "None")
                {
                    Bind.Text += hotkey[3];
                }

                Binds.Add(hotkey);

                Translation.Text = TranslationTextbox.Text;
                Translation.Padding = new Thickness(5, 5, 5, 5);
                Bind.Padding = new Thickness(5, 5, 5, 5);
                int fontSize = 20;
                Translation.FontSize = fontSize;
                Bind.FontSize = fontSize;
                Translation.FontFamily = new FontFamily("Montserrat Light");
                Bind.FontFamily = new FontFamily("Montserrat Light");

                GeekerGrid.Children.Add(Translation);
                GeekerGrid.Children.Add(Bind);

                Grid.SetColumn(Translation, 0);
                Grid.SetColumn(Bind, 1);

                Grid.SetRow(Translation, GeekerGrid.RowDefinitions.Count);
                Grid.SetRow(Bind, GeekerGrid.RowDefinitions.Count);

                CtrlDropdown.SelectedIndex = -1;
                ShiftDropdown.SelectedIndex = -1;
                AltDropdown.SelectedIndex = -1;
                LetterDropdown.SelectedIndex = -1;
                TranslationTextbox.Text = "";

            }
            else
            {
                MessageBox.Show("Make sure all the textboxes are filled in before adding the term.");
            }
        }

        private string[] getHotkey(string text)
        {
            string[] hotkey = { "None", "None", "None", "None", text };
            if (CtrlDropdown.SelectedIndex == 1)
            {
                hotkey[0] = "Ctrl";
            }
            if (ShiftDropdown.SelectedIndex == 1)
            {
                hotkey[1] = "Shift";
            }
            if (AltDropdown.SelectedIndex == 1)
            {
                hotkey[2] = "Alt";
            }

            if (LetterDropdown.SelectedIndex == 0)
            {
                hotkey[3] = "A";
            }
            if (LetterDropdown.SelectedIndex == 1)
            {
                hotkey[3] = "B";
            }
            if (LetterDropdown.SelectedIndex == 2)
            {
                hotkey[3] = "C";
            }
            if (LetterDropdown.SelectedIndex == 3)
            {
                hotkey[3] = "D";
            }
            if (LetterDropdown.SelectedIndex == 4)
            {
                hotkey[3] = "E";
            }
            if (LetterDropdown.SelectedIndex == 5)
            {
                hotkey[3] = "F";
            }
            if (LetterDropdown.SelectedIndex == 6)
            {
                hotkey[3] = "G";
            }
            if (LetterDropdown.SelectedIndex == 7)
            {
                hotkey[3] = "H";
            }
            if (LetterDropdown.SelectedIndex == 8)
            {
                hotkey[3] = "I";
            }
            if (LetterDropdown.SelectedIndex == 9)
            {
                hotkey[3] = "J";
            }
            if (LetterDropdown.SelectedIndex == 10)
            {
                hotkey[3] = "K";
            }
            if (LetterDropdown.SelectedIndex == 11)
            {
                hotkey[3] = "L";
            }
            if (LetterDropdown.SelectedIndex == 12)
            {
                hotkey[3] = "M";
            }
            if (LetterDropdown.SelectedIndex == 13)
            {
                hotkey[3] = "N";
            }
            if (LetterDropdown.SelectedIndex == 14)
            {
                hotkey[3] = "O";
            }
            if (LetterDropdown.SelectedIndex == 15)
            {
                hotkey[3] = "P";
            }
            if (LetterDropdown.SelectedIndex == 16)
            {
                hotkey[3] = "Q";
            }
            if (LetterDropdown.SelectedIndex == 17)
            {
                hotkey[3] = "R";
            }
            if (LetterDropdown.SelectedIndex == 18)
            {
                hotkey[3] = "S";
            }
            if (LetterDropdown.SelectedIndex == 19)
            {
                hotkey[3] = "T";
            }
            if (LetterDropdown.SelectedIndex == 20)
            {
                hotkey[3] = "U";
            }
            if (LetterDropdown.SelectedIndex == 21)
            {
                hotkey[3] = "V";
            }
            if (LetterDropdown.SelectedIndex == 22)
            {
                hotkey[3] = "W";
            }
            if (LetterDropdown.SelectedIndex == 23)
            {
                hotkey[3] = "X";
            }
            if (LetterDropdown.SelectedIndex == 24)
            {
                hotkey[3] = "Y";
            }
            if (LetterDropdown.SelectedIndex == 25)
            {
                hotkey[3] = "Z";
            }
            return hotkey;
        }

        private bool isActivated = false;
        
        private void Activate_Click(object sender, RoutedEventArgs e)
        {
            
            if (!isActivated)
            {
                isActivated = true;
                ActivateButton.Content = "Gëeking";

                foreach(string[] binded in Binds)
                {
                    ModifierKeys[] modiferKeysArray = getModiferKeys(binded);

                    Key letter = getLetterKey(binded);
                    
                    var key = new HotKey(
                                    modiferKeysArray[0] | modiferKeysArray[1] | modiferKeysArray[2],
                                    letter,
                                    this,
                                    (hotkey) => {
                                        sim.Keyboard.Sleep(20);
                                        sim.Keyboard.TextEntry(binded[4]);
                                    }
                                );
                    keys.Add(key);
                }
            } 
            else
            {
                isActivated = false;
                ActivateButton.Content = "Boot üp";
                foreach(HotKey key in keys)
                {
                    key.Dispose();
                }
            }
        }

        private Key getLetterKey(string[] binded)
        {
            Key letterkey = Key.A;
            switch (binded[3])
            {
                case "A":
                    letterkey = Key.A;
                    break;
                case "B":
                    letterkey = Key.B;
                    break;
                case "C":
                    letterkey = Key.C;
                    break;
                case "D":
                    letterkey = Key.D;
                    break;
                case "E":
                    letterkey = Key.E;
                    break;
                case "F":
                    letterkey = Key.F;
                    break;
                case "G":
                    letterkey = Key.G;
                    break;
                case "H":
                    letterkey = Key.H;
                    break;
                case "I":
                    letterkey = Key.I;
                    break;
                case "J":
                    letterkey = Key.J;
                    break;
                case "K":
                    letterkey = Key.K;
                    break;
                case "L":
                    letterkey = Key.L;
                    break;
                case "M":
                    letterkey = Key.M;
                    break;
                case "N":
                    letterkey = Key.N;
                    break;
                case "O":
                    letterkey = Key.O;
                    break;
                case "P":
                    letterkey = Key.P;
                    break;
                case "Q":
                    letterkey = Key.Q;
                    break;
                case "R":
                    letterkey = Key.R;
                    break;
                case "S":
                    letterkey = Key.S;
                    break;
                case "T":
                    letterkey = Key.T;
                    break;
                case "U":
                    letterkey = Key.U;
                    break;
                case "V":
                    letterkey = Key.V;
                    break;
                case "W":
                    letterkey = Key.W;
                    break;
                case "X":
                    letterkey = Key.X;
                    break;
                case "Y":
                    letterkey = Key.Y;
                    break;
                case "Z":
                    letterkey = Key.Z;
                    break;
            }
            return letterkey;
        }

        private ModifierKeys[] getModiferKeys(string[] binded)
        {
            ModifierKeys[] modiferKeysArray = { ModifierKeys.None, ModifierKeys.None, ModifierKeys.None};
            if (binded[0] == "Ctrl")
            {
                modiferKeysArray[0] = ModifierKeys.Control;
            }
            if (binded[1] == "Shift")
            {
                modiferKeysArray[1] = ModifierKeys.Shift;
            }
            if (binded[2] == "Alt")
            {
                modiferKeysArray[2] = ModifierKeys.Alt;
            }
            return modiferKeysArray;
        }
    }

}
