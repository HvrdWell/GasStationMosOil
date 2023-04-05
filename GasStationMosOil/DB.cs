using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySqlConnector;
using MySqlX.XDevAPI.Relational;

namespace GasStationMosOil
{
    class DB
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;password=localhost;database=testdb;");
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();

        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public MySqlConnection getConnection()
        {
            return connection;
        }

        public int RoleStringToId(String roleName)
        {
            string sql = "SELECT userroles.id FROM userroles WHERE userroles.`name` = @NameOfRole";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@NameOfRole", MySqlDbType.VarChar).Value = roleName;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return Convert.ToInt32(table);
        }
        public int GenderStringToId(String gender)
        {
            string sql = "SELECT usergender.id FROM userroles WHERE userroles.`name` = @NameOfRole";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@NameOfRole", MySqlDbType.VarChar).Value = gender;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return Convert.ToInt32(table);
        }
        public int StatusStringToId(String gender)
        {
            string sql = "SELECT userstatus.id FROM userroles WHERE userroles.`name` = @NameOfRole";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@NameOfRole", MySqlDbType.VarChar).Value = gender;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return Convert.ToInt32(table);
        }
        public DataTable infoAboutUser(int idOfUser)
        {
            string sql = "SELECT `user`.* FROM `user` WHERE `user`.idUser = @id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = idOfUser;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return table;
        }

        public DataTable StatusAll()
        {
            string sql = "SELECT `userstatus`.* FROM `userstatus`";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return table;
        }

        public DataTable roleAll()
        {
            string sql = "SELECT `userroles`.* FROM `userroles`";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return table;
        }

        public DataTable genderAll()
        {
            string sql = "SELECT `usergenders`.name,`usergenders`.id  FROM `usergenders`";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return table;
        }

        public DataTable OrderStatus()
        {
            string sql = "SELECT `statusorder`.* FROM `statusorder`";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return table;
        }
        public DataTable bonusCards()
        {
            string sql = "SELECT `bonuscard`.* FROM `bonuscard`";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return table;
        }
        public bool insertUserRole(string name)
        {
            string sql = "insert into userroles(`name`) Values(@Name)";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return true;
        }
        public bool insertOrderStatus(string name)
        {
            string sql = "insert into statusorder(`name`) Values(@Name)";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return true;
        }
        public bool insertbonusCards(int name)
        {
            string sql = "insert into bonuscard(`scores`) Values(@Name)";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.Int32).Value = name;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return true;
        }
        public bool insertGender(string name)
        {
            string sql = "insert into usergenders(`name`) Values(@Name)";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return true;
        }
        public bool insertUserStatus(string name)
        {
            string sql = "insert into userstatus(`name`) Values(@Name)";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return true;
        }
        public bool insertRole(string name)
        {
            string sql = "insert into userroles(`name`) Values(@Name)";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return true;
        }

        public void UpdateRole(int id, string name)
        {
            string sql = "UPDATE userroles SET name = @Name WHERE id = @Id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

        }
        public void UpdateUserStatus(int id, string name)
        {
            string sql = "UPDATE userstatus SET name = @Name WHERE id = @Id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

        }
        public void UpdateUserRoles(int id, string name)
        {
            string sql = "UPDATE userroles SET name = @Name WHERE id = @Id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

        }
        public void UpdateGender(int id, string name)
        {

            string sql = "UPDATE usergenders SET name = @Name WHERE id = @Id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

        }
        public void UpdatebonusCards(int id, string scores)
        {
            string sql = "UPDATE bonuscard SET scores = @Scores WHERE id = @Id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Scores", MySqlDbType.VarChar).Value = scores;
            cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

        }

        public void UpdateGoods(int id, string name, float price, int count)
        {
            string sql = "UPDATE product SET name = @Name, price = @Price, count = @Count  WHERE id = @Id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@Price", MySqlDbType.VarChar).Value = price;
            cmd.Parameters.Add("@Count", MySqlDbType.VarChar).Value = count;
            cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

        }
        public DataTable infoAboutGood(int idOfGood)
        {
            string sql = "SELECT `product`.* FROM `product` WHERE `product`.id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = idOfGood;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return table;
        }
        public bool insertProduct(string name, float price, int count)
        {
            string sql = "insert into product(name,price,count) Values(@Name,@Price,@count)";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@Price", MySqlDbType.VarChar).Value = price;
            cmd.Parameters.Add("@count", MySqlDbType.VarChar).Value = count;

            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return true;
        }
        public void deleteGood(int idOfGood)
        {
            string sql = "delete product FROM product WHERE product.id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = idOfGood;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);
        }
        public void UpdateTanks(int id, string name, float price)
        {
            string sql = "UPDATE typefuel SET nameTypeFuel = @Name, price = @Price  WHERE idTypeFuel = @Id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@Price", MySqlDbType.VarChar).Value = price;
            cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);
        }
        public void UpdateStorage(int id, float count)
        {
            string sql = "UPDATE storage SET count = @Count WHERE idTypeFuel = @Id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Count", MySqlDbType.VarChar).Value = count;
            cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

        }
        public DataTable infoAboutTanks(int idOfTanks)
        {
            string sql = "SELECT typefuel.idTypeFuel, typefuel.nameTypeFuel, typefuel.price, `storage`.count FROM typefuel INNER JOIN `storage` ON typefuel.idTypeFuel = `storage`.idTypeFuel WHERE typefuel.idTypeFuel = `storage`.idTypeFuel AND typefuel.idTypeFuel = @id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = idOfTanks;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return table;
        }

        public String nameOfUserRole(int idOfRole)
        {
            string sql = "SELECT userroles.`name` FROM userroles WHERE userroles.id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = idOfRole;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return Convert.ToString(table.Rows[0][0]);
        }
        public int NewOrder(int Columns, int User, decimal totalCountFuel, decimal totalPrice)
        {
            DateTime Time = DateTime.Now;
            string sql = "insert into `order`(`idColumns`,`idUser`,`data`, `totalCountFuel`, `totalPrice`) Values(@idColumns,@idUser,@data,@totalCountFuel,@totalPrice)";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@idColumns", MySqlDbType.Int32).Value = Columns;
            cmd.Parameters.Add("@idUser", MySqlDbType.Int32).Value = User;
            cmd.Parameters.Add("@data", MySqlDbType.DateTime).Value = Time;
            cmd.Parameters.Add("@totalCountFuel", MySqlDbType.Decimal).Value = totalCountFuel;
            cmd.Parameters.Add("@totalPrice", MySqlDbType.Decimal).Value = totalPrice;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);
            return (int)cmd.LastInsertedId;
        }


        public bool ifEnoughTank(int idOfFuelType, double literCount)
        {
            string sql = "SELECT count FROM `storage` WHERE idTypeFuel = @idType";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@idType", MySqlDbType.VarChar).Value = idOfFuelType;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);
            if (Convert.ToDouble(table.Rows[0][0]) < literCount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public decimal howManyCountOfTank(int idOfFuelType)
        {
            string sql = "SELECT count FROM `storage` WHERE idTypeFuel = @idType";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@idType", MySqlDbType.VarChar).Value = idOfFuelType;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);
            return Convert.ToDecimal(table.Rows[0][0]);
        }

        public void MinusCount(int idOfFuelType, decimal count)
        {
            string sql = "UPDATE `storage` SET count = @Count  WHERE idTypeFuel = @Id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Count", MySqlDbType.VarChar).Value = howManyCountOfTank(idOfFuelType) - count;
            decimal a = howManyCountOfTank(idOfFuelType) - count;
            cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = idOfFuelType;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);
        }
        public int findProductById(int idOfProduct)
        {
            string sql = "SELECT `product`.count FROM `product` WHERE `product`.id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = idOfProduct;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            return Convert.ToInt32(table.Rows[0][0]);
        }
        public void deleteCountOfProduct(int idOfProduct, int count)
        {
            string sql = "UPDATE `product` SET count = @Count  WHERE id = @Id";
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = idOfProduct;
            cmd.Parameters.Add("@Count", MySqlDbType.VarChar).Value = findProductById(idOfProduct) - count;

            adapter.SelectCommand = cmd;
            adapter.Fill(table);
        }
        public DataTable selectNozzlePosts(int number)
        {
            string sql = ("SELECT `storage`.idTypeFuel AS id, typefuel.nameTypeFuel AS `Name`, `storage`.count AS Reserve, typefuel.price AS Price, `column`.number FROM typefuel INNER JOIN `storage` ON typefuel.idTypeFuel = `storage`.idTypeFuel INNER JOIN `column` ON `storage`.id = `column`.idStorage WHERE `storage`.idTypeFuel = typefuel.idTypeFuel AND `column`.idStorage = `storage`.id AND `column`.number = @NUMBER");
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@NUMBER", MySqlDbType.VarChar).Value = number;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);
            return (table);
        }
        public void addToBucketProduct(int idOfProduct, int count, int idOrder)
        {
            string sql = ("insert into `foodbucket` (`idProduct`, `count`, `idOrder`) Values (@idProduct, @count, @idOrder)");
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@idProduct", MySqlDbType.VarChar).Value = idOfProduct;
            cmd.Parameters.Add("@count", MySqlDbType.VarChar).Value = count;
            cmd.Parameters.Add("@idOrder", MySqlDbType.VarChar).Value = idOrder;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);
            return;
        }
        public int findProductByBarCode(string barCode)
        {
            string sql = ("SELECT product.id FROM product WHERE product.barCode = @BarCode");
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@BarCode", MySqlDbType.VarChar).Value = barCode;

            adapter.SelectCommand = cmd;
            adapter.Fill(table);
            return Convert.ToInt32(table.Rows[0][0]);
        }
        public int howManyProductsCount(int id)
        { 
            string sql = ("SELECT count FROM product WHERE product.id = @Id");
            MySqlCommand cmd = new MySqlCommand(sql, getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
            adapter.SelectCommand = cmd;
            adapter.Fill(table);
            return Convert.ToInt32(table.Rows[1][1]);
        }
    }
}
