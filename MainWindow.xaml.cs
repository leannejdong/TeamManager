using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeamManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TeamDetails> teamList = new List<TeamDetails>();
        FileManager file = new FileManager();
        bool isNewEntry = true;

        private List<TeamDetails> entries;
        //private int points;
  /*      public void SetPoints(int points)
        {
            this.points = points;
        }*/
        public MainWindow()
        {
            InitializeComponent();
            entries = new List<TeamDetails>();
            teamList = file.ReadDataFromFile();
            UpdateTableView();
            ClearEntryFields("Add details here");
        }
       
        #region Events

        /// <summary>
        /// Method which triggers whenever the save button is pressed
        /// </summary>
        /// <param name="sender">The object triggering the event</param>
        /// <param name="e">Event parameters passsed when triggered</param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            //if (int.TryParse(btnSave.Tag.ToString(), out int points))
            if(int.TryParse(txtPoints.Text, out int points))
            {
               // btnSave.Tag = points;
                    
                SaveNewEntry(points);
            }
            else
            {
                MessageBox.Show("Invalid points value. Please enter a valid integer.");
            }
         }
            

        /// <summary>
        /// Checks if the enter key is pressed when the curson is within the form section.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                // Convert the text in txtPoints to an integer
                if (int.TryParse(txtPoints.Text, out int points))
                {
                    // Set the Tag property of the btnSave button with the points value
                    btnSave.Tag = points;

                    // Debugging: Check if the Tag property is set correctly
                    Debug.WriteLine($"Tag property of btnSave set to: {btnSave.Tag}");

                    // Call the btnSave_Click event handler to save the entry
                    btnSave_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Invalid points value. Please enter a valid integer.");
                }
            }
        }

        /// <summary>
        /// When any of the associated fields get the focus and the current text is the placeholder value, all the form
        /// fields will be cleared and ready for new inputs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_GotFocus(object sender, RoutedEventArgs e)
        {
            //Get the source of the event and store it as a textfield. Conversion is needed from the object type.
            TextBox textbox = (TextBox)sender;

            //If the current text is the placeholder string, run the clear fields method.
            if (textbox.Text.Equals("Add details here"))
            {
                ClearEntryFields();
            }
        }

        /// <summary>
        /// Event triggered by the Clear Button which runs the associated method call.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearEntryFields();
        }

        /// <summary>
        /// Event triggered by the Delete Button which runs the associated method call.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedEntry();
        }

        /// <summary>
        /// Event triggered by the Exit Button which runs the associated code.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //Shuts down the application.
            Application.Current.Shutdown();
        }

        private void dgvAddressTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvAddressTable.SelectedIndex < 0)
            {
                return;
            }
            int id = dgvAddressTable.SelectedIndex;

            TeamDetails selectedTeam = (TeamDetails)dgvAddressTable.SelectedItem;
            txtTeam.Text = selectedTeam.TeamName;
            txtPrimary.Text = selectedTeam.PrimaryContact;
            txtPhone.Text = selectedTeam.ContactPhone;
            txtEmail.Text = selectedTeam.ContactEmail;
            txtPoints.Text = selectedTeam.CompetitionPoints.ToString();

            isNewEntry = false;
        }



        /// <summary>
        /// Event triggered by changing the selected row in the data grid which runs the associated code.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        #endregion



        /// <summary>
        /// Saves the details to the file.
        /// </summary>
        private void SaveNewEntry(int txtPoints)
        {
            //Checks if the form is filled before saving. 
            //If not it pops up a message to the user and then exits the method.
            if (!IsFormFilledCorrectly())
            {
                MessageBox.Show("Form not filled correctly!\n PLease check and try again.");
                return;
            }

            // Retrieve competition points value
            if (!int.TryParse(txtPoints.ToString(), out int points) || points < 0)
            {
                MessageBox.Show("Competition points must be a positive integer.");
                return;
            }

            //Writes all the provided text into a new Address object.
            TeamDetails newEntry = new TeamDetails();
            //Reads each text field and gets its text property and puts it into the object property
            newEntry.TeamName = txtTeam.Text;
            newEntry.PrimaryContact = txtPrimary.Text;
            newEntry.ContactEmail = txtEmail.Text;
            newEntry.ContactPhone = txtPhone.Text;
            newEntry.CompetitionPoints = txtPoints;

            //If new entry flag is set to true, saves a new entry to the list. Otherwise, overwrites the existing entry.
            if (isNewEntry)
            {
                //Adds the new object to the list
                teamList.Add(newEntry);
            }
            else
            {
                //Goes to the index of the currently selected entry in the table and overwrites it.
                teamList[dgvAddressTable.SelectedIndex] = newEntry;
            }

            //Saves the current list to file
            file.WriteDataToFile(teamList.ToArray());

            UpdateTableView();

            ClearEntryFields();

            //Puts ther focus(cursor) back to the first text field
            // txtPoints.Focus();
            txtTeam.Focus();

        }

        private void ClearEntryFields()
        {
            txtTeam.Text = string.Empty;
            txtPrimary.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPoints.Text = string.Empty;
            isNewEntry = true;
        }

        private void ClearEntryFields(string text)
        {
            txtTeam.Text = text;
            txtPrimary.Text = text;
            txtPhone.Text = text;
            txtEmail.Text = text;
            txtPoints.Text = text;
        }

        private bool IsFormFilledCorrectly()
        {
            if (string.IsNullOrWhiteSpace(txtTeam.Text))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPrimary.Text))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                return false;
            }

            return true;
        }

        private void UpdateTableView()
        {
            dgvAddressTable.ItemsSource = teamList;
            dgvAddressTable.Items.Refresh();
        }

        private void DeleteSelectedEntry()
        {
            //Checks if an entry in the data grid has been selected and is currently highlighted.
            if (dgvAddressTable.SelectedIndex < 0)
            {
                //Exits out of the method if no entry selected.
                return;
            }

            //Opens a message box for the user to confirm the deletion and stores their response.
            MessageBoxResult selection = MessageBox.Show("Are you sure you want to delete this?", "Confirmation", MessageBoxButton.YesNo);
            //If user said yes to delete confrimation, the delete p[rocess goes ahead.
            if (selection == MessageBoxResult.Yes)
            {
                //Grabs the index number of the selected row in the data grid.
                //This index will align with the matching entry in the list. 
                int selected = dgvAddressTable.SelectedIndex;
                //Removes the list entry at the specified index.
                teamList.RemoveAt(selected);

                //Updates on screen forms and data grid before savbing changes to file.
                UpdateTableView();
                ClearEntryFields();
                file.WriteDataToFile(teamList.ToArray());
            }


        }

        private void txtPoints_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Check if the entered text is a valid integer
            if (!int.TryParse(e.Text, out _))
            {
                // Cancel the input if it is not a valid integer
                e.Handled = true;
            }
        }


        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgvAddressTable.SelectedIndex >= 0)
            {
                // Get the selected index
                int selectedIndex = dgvAddressTable.SelectedIndex;

                // Populate the form fields with the selected entry's data
                txtTeam.Text = teamList[selectedIndex].TeamName;
                txtPrimary.Text = teamList[selectedIndex].PrimaryContact;
                txtPhone.Text = teamList[selectedIndex].ContactPhone;
                txtEmail.Text = teamList[selectedIndex].ContactEmail;
                txtPoints.Text = teamList[selectedIndex].CompetitionPoints.ToString();

                // Set isNewEntry flag to false as we are editing an existing entry
                isNewEntry = false;
            }
            else
            {
                // Show a message if no entry is selected
                MessageBox.Show("Please select an entry to edit.");
            }


        }

        private FileManager fileManager = new FileManager();

        // Method to load data from CSV file
        private void LoadDataFromCSV()
        {
            List<TeamDetails> teamList = file.ReadDataFromFile();
            // Populate your UI with the data from teamList
        }

        // Method to save data to CSV file
        private void SaveDataToCSV(List<TeamDetails> teamList)
        {
            fileManager.WriteDataToFile(teamList);
        }





    }
}