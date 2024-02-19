
using System.IO;
using System.Text.Json;
using System.Windows;


namespace superezhednevnik
{
    public class Note
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }


    public partial class MainWindow : Window
    {
        private List<Note> notes;
        private string filePath = "notes.json"; // Путь к файлу заметок

        public MainWindow()
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Today;
            LoadNotesFromFile();
        }

        private void ShowNotes_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Today;
            ShowNotesForDate(selectedDate);
        }

        private void ShowNotesForDate(DateTime date)
        {
            notesListBox.ItemsSource = null;
            List<Note> notesForDate = new List<Note>();

            foreach (var note in notes)
            {
                if (note.DueDate.Date == date.Date)
                {
                    notesForDate.Add(note);
                }
            }

            notesListBox.ItemsSource = notesForDate;
        }

        private void LoadNotesFromFile()
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                notes = JsonSerializer.Deserialize<List<Note>>(jsonData);
            }
            else
            {
                notes = new List<Note>();
            }
        }

        private void SaveNotesToFile()
        {
            string jsonData = JsonSerializer.Serialize(notes);
            File.WriteAllText(filePath, jsonData);
        }

        private void AddNote()
        {
            NoteDialog dialog = new NoteDialog();
            if (dialog.ShowDialog() == true)
            {
                notes.Add(dialog.Note);
                SaveNotesToFile();
                ShowNotesForDate(datePicker.SelectedDate ?? DateTime.Today);
            }
        }

        private void EditNote()
        {
            Note selectedNote = (Note)notesListBox.SelectedItem;
            if (selectedNote != null)
            {
                NoteDialog dialog = new NoteDialog(selectedNote);
                if (dialog.ShowDialog() == true)
                {
                    // Update note
                    int index = notes.IndexOf(selectedNote);
                    notes[index] = dialog.Note;
                    SaveNotesToFile();
                    ShowNotesForDate(datePicker.SelectedDate ?? DateTime.Today);
                }
            }
            else
            {
                MessageBox.Show("Please select a note to edit.");
            }
        }

        private void DeleteNote()
        {
            Note selectedNote = (Note)notesListBox.SelectedItem;
            if (selectedNote != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this note?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    notes.Remove(selectedNote);
                    SaveNotesToFile();
                    ShowNotesForDate(datePicker.SelectedDate ?? DateTime.Today);
                }
            }
            else
            {
                MessageBox.Show("Please select a note to delete.");
            }
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            AddNote();
        }

        private void EditNote_Click(object sender, RoutedEventArgs e)
        {
            EditNote();
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            DeleteNote();
        }
    }
}
