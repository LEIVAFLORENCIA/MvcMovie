using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using Microsoft.Data.SqlClient;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = MoviesADO; Integrated Security = True; Connect Timeout = 30; Encrypt=False;";
        public static List<Movie> listMovies = new List<Movie>();

        // GET: Movies
        public async Task<IActionResult> Index()
        {

            try
            {
                listMovies.Clear();
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string SELECT_MOVIES = "SELECT * FROM Movies";
                SqlCommand command = new SqlCommand(SELECT_MOVIES, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Movie movie = new Movie()
                    {
                        Id = int.Parse(reader[0].ToString()),
                        Genre = reader[1].ToString(),
                        Price = (decimal) Double.Parse(reader[2].ToString()),
                        ReleaseDate = DateTime.Parse(reader[3].ToString()),
                        Title = reader[4].ToString()
                    };
                    listMovies.Add(movie);
                }
                connection.Close();
                return View(listMovies);
            }
            catch (Exception ex)
            {
                throw;
            }        
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // TODO: Agregar una vista
            }

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string SELECT_MOVIE_BY_ID = "SELECT * FROM Movies where id=@Id;";
                SqlCommand command = new SqlCommand(SELECT_MOVIE_BY_ID, connection);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();
                Movie movie = null;
                while (reader.Read())
                {
                    movie = new Movie()
                    {
                        Id = (int)reader[0],
                        Genre = reader[1].ToString(),
                        Price = (decimal)Double.Parse(reader[2].ToString()),
                        ReleaseDate = (DateTime)reader[3],
                        Title = reader[4].ToString()
                    };
                }
                connection.Close();
                return View(movie);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult Create()
        {
            var model = new Movie();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();
                string INSERT_MOVIE = "INSERT INTO Movies (Genre, Price, ReleaseDate, Title) VALUES (@Genre, @Price, @ReleaseDate, @Title);";
                SqlCommand command = new SqlCommand(INSERT_MOVIE, connection);
                command.Parameters.AddWithValue("@Genre", movie.Genre);
                command.Parameters.AddWithValue("@Price", movie.Price);
                command.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate);
                command.Parameters.AddWithValue("@Title", movie.Title);

                // Ejecuta la consulta
                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
                return RedirectToAction("Index");

            }catch(Exception ex)
            {
                throw;
            }
        }
        
        public ActionResult Delete(int? id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string DELETE_MOVIE = "DELETE FROM Movies where id=@Id;";
                SqlCommand command = new SqlCommand(DELETE_MOVIE, connection);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
