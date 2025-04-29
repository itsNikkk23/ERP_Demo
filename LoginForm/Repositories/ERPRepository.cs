using System.Data.SqlClient;
using System.Data;
using LoginForm.Models;
using LoginForm.Data;
using Microsoft.Data.SqlClient;
namespace LoginForm.Repository
{
    public class ERPRepository : IERPRepository
    {

        private readonly DbHelper _dbHelper;

        public ERPRepository(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        // Retrieve all raw materials
        public bool ValidateCredentials(string UserName, string Passcode)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                const string sql = @"
                           SELECT COUNT(*) 
                          FROM UserSignup 
                          WHERE UserName = @UserName 
                          AND Passcode = @Passcode";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Passcode", Passcode);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                con.Close();
                return count > 0;
            }
        }
        public void LogLogin(string UserName, string Passcode)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                const string sql = @"
                           INSERT INTO UserLogin (UserName, Passcode) 
                          VALUES (@UserName, @Passcode)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Passcode", Passcode);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public List<RawMaterials> GetAllRawMaterials()
        {
            List<RawMaterials> rawMaterialsList = new List<RawMaterials>();

            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Manufacture.RawMaterials";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    rawMaterialsList.Add(new RawMaterials
                    {
                        RawMaterialID = Convert.ToInt32(reader["RawMaterialID"]),
                        MaterialName = reader["MaterialName"].ToString(),
                        MaterialType = reader["MaterialType"].ToString(),
                        UnitOfMeasure = reader["UnitOfMeasure"].ToString(),
                        Description = reader["Description"].ToString()
                    });
                }
                conn.Close();
            }

            return rawMaterialsList;
        }
        public void InsertPurchaseOrder(int SupplierID, DateTime OrderDate, DateTime DeliveryDate, decimal TotalAmount, string Status)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("Manufacture.InsertPurchaseOrder", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmd.Parameters.AddWithValue("@OrderDate", OrderDate);
                cmd.Parameters.AddWithValue("@DeliveryDate", DeliveryDate);
                cmd.Parameters.AddWithValue("@TotalAmount", TotalAmount);
                cmd.Parameters.AddWithValue("@Status", Status);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public PurchaseOrders GetPurchaseOrderById(int orderID)
        {
            PurchaseOrders order = null;
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("GetPurchaseOrderById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderID", orderID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    order = new PurchaseOrders
                    {
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        SupplierID = Convert.ToInt32(reader["SupplierID"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        DeliveryDate = reader["DeliveryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DeliveryDate"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        Status = reader["Status"].ToString()
                    };
                }
            }
            return order;
        }

        public void UpdatePurchaseOrder(int OrderID, int SupplierID, DateTime OrderDate, DateTime? DeliveryDate, decimal TotalAmount, string Status)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("UpdatePurchaseOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderID", OrderID);
                cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmd.Parameters.AddWithValue("@OrderDate", OrderDate);
                cmd.Parameters.AddWithValue("@DeliveryDate", (object?)DeliveryDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TotalAmount", TotalAmount);
                cmd.Parameters.AddWithValue("@Status", Status);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeletePurchaseOrder(int OrderID)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("DeletePurchaseOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderID", OrderID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        // Retrieve a single raw material by ID
        public RawMaterials GetRawMaterialById(int rawMaterialId)
        {
            RawMaterials material = null;
            using (SqlConnection conn = _dbHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("GetRawMaterialById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RawMaterialID", rawMaterialId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    material = new RawMaterials
                    {
                        RawMaterialID = Convert.ToInt32(reader["RawMaterialID"]),
                        MaterialName = reader["MaterialName"].ToString(),
                        MaterialType = reader["MaterialType"].ToString(),
                        UnitOfMeasure = reader["UnitOfMeasure"].ToString(),
                        Description = reader["Description"].ToString()
                    };
                }
            }
            return material;
        }

        public void InsertRawMaterials(string MaterialName, string MaterialType, string UnitOfMeasure, string Description)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_InsertRawMaterial", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaterialName", MaterialName);
                cmd.Parameters.AddWithValue("@MaterialType", MaterialType);
                cmd.Parameters.AddWithValue("@UnitOfMeasure", UnitOfMeasure);
                cmd.Parameters.AddWithValue("@Description", Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateRawMaterials(int RawMaterialID, string MaterialName, string MaterialType, string UnitOfMeasure, string Description)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                // string query = "UPDATE Manufacture.RawMaterials SET MaterialName = @MaterialName, MaterialType = @MaterialType, UnitOfMeasure = @UnitOfMeasure, Description = @Description WHERE RawMaterialID = @RawMaterialID";
                SqlCommand cmd = new SqlCommand("UpdateRawMaterial", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RawMaterialID", RawMaterialID);
                cmd.Parameters.AddWithValue("@MaterialName", MaterialName);
                cmd.Parameters.AddWithValue("@MaterialType", MaterialType);
                cmd.Parameters.AddWithValue("@UnitOfMeasure", UnitOfMeasure);
                cmd.Parameters.AddWithValue("@Description", Description);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void DeleteRawMaterials(int RawMaterialID)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                // string query = "DELETE FROM Manufacture.RawMaterials WHERE RawMaterialID = @RawMaterialID";
                SqlCommand cmd = new SqlCommand("DeleteRawMaterial", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RawMaterialID", RawMaterialID);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void InsertSuppliers(string SupplierName, string SupplierContact)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Supply.Suppliers (SupplierName, SupplierContact) VALUES (@SupplierName, @SupplierContact)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SupplierName", SupplierName);
                cmd.Parameters.AddWithValue("@SupplierContact", SupplierContact);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }
        public Suppliers GetSupplierById(int SupplierID)
        {
            Suppliers supplier = null;
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Supply.Suppliers WHERE SupplierID = @SupplierID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    supplier = new Suppliers
                    {
                        SupplierID = Convert.ToInt32(reader["SupplierID"]),
                        SupplierName = reader["SupplierName"].ToString(),
                        SupplierContact = reader["SupplierContact"].ToString()
                    };
                }
                conn.Close();
            }
            return supplier;
        }
        public List<Suppliers> GetAllSuppliers()
        {
            List<Suppliers> suppliersList = new List<Suppliers>();

            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Supply.Suppliers";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    suppliersList.Add(new Suppliers
                    {
                        SupplierID = Convert.ToInt32(reader["SupplierID"]),
                        SupplierName = reader["SupplierName"].ToString(),
                        SupplierContact = reader["SupplierContact"].ToString()
                    });
                }
                conn.Close();
            }

            return suppliersList;
        }
        //public Suppliers GetSupplierById(int SupplierID)
        //{
        //    Suppliers supplier = null;
        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        string query = "SELECT * FROM Supply.Suppliers WHERE SupplierID = @SupplierID";
        //        SqlCommand cmd = new SqlCommand("GetSupplierByID", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            supplier = new Suppliers
        //            {
        //                SupplierId = Convert.ToInt32(reader["SupplierID"]),
        //                SupplierName = reader["SupplierName"].ToString(),
        //                SupplierContact = reader["SupplierContact"].ToString()
        //            };
        //        }
        //        conn.Close();
        //    }
        //    return supplier;
        //}
        public void UpdateSuppliers(int SupplierId, string SupplierName, string SupplierContact)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                //string query = "UPDATE Supply.Suppliers SET SupplierName = @SupplierName, SupplierContact = @SupplierContact WHERE SupplierID = @SupplierID";
                SqlCommand cmd = new SqlCommand("UpdateSupplier", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierID", SupplierId);
                cmd.Parameters.AddWithValue("@SupplierName", SupplierName);
                cmd.Parameters.AddWithValue("@SupplierContact", SupplierContact);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void DeleteSuppliers(int SupplierId)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                //string query = "DELETE FROM Supply.Suppliers WHERE SupplierID = @SupplierID";
                SqlCommand cmd = new SqlCommand("DeleteSupplier", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierID", SupplierId);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        //public void InsertPurchaseOrder(int SupplierID, DateTime OrderDate, DateTime DeliveryDate, Decimal TotalAmount, string Status)
        //{
        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        string query = "INSERT INTO Manufacture.PurchaseOrders (SupplierID, OrderDate, DeliveryDate, TotalAmount, Status) VALUES (@SupplierID, @OrderDate, @DeliveryDate, @TotalAmount, @Status)";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
        //        cmd.Parameters.AddWithValue("@OrderDate", OrderDate);
        //        cmd.Parameters.AddWithValue("@DeliveryDate", DeliveryDate);
        //        cmd.Parameters.AddWithValue("@TotalAmount", TotalAmount);
        //        cmd.Parameters.AddWithValue("@Status", Status);
        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //        conn.Close();
        //    }
        //}
        public List<PurchaseOrders> GetAllPurchaseOrders()
        {
            List<PurchaseOrders> orders = new List<PurchaseOrders>();
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Manufacture.PurchaseOrders";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new PurchaseOrders
                    {
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        SupplierID = Convert.ToInt32(reader["SupplierID"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        DeliveryDate = reader["DeliveryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DeliveryDate"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        Status = reader["Status"].ToString()
                    });
                }
                conn.Close();
            }
            return orders;
        }

        public void AddWeaving(string weave_type, string loomtype)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                //string query = "INSERT INTO Manufacture.Weaving (weave_type, loomtype) VALUES (@weave_type, @loomtype)";
                SqlCommand cmd = new SqlCommand("InsertWeavingProcess", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@weave_type", weave_type);
                cmd.Parameters.AddWithValue("@loomtype", loomtype);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public List<Weaving_process> GetWeavingProcess()
        {
            List<Weaving_process> weavingList = new List<Weaving_process>();
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                //string query = "SELECT * FROM Manufacture.Weaving";
                SqlCommand cmd = new SqlCommand("GetWeavingProcess", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    weavingList.Add(new Weaving_process
                    {
                        WeaveID = Convert.ToInt32(reader["WeaveID"]),
                        weave_type = reader["weave_type"].ToString(),
                        loomType = reader["loomtype"].ToString()
                    });
                }
                conn.Close();
            }
            return weavingList;
        }
        public void UpdateWeaving(int WeaveID, string weave_type, string loomtype)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                //string query = "UPDATE Manufacture.Weaving SET weave_type = @weave_type, loomtype = @loomtype WHERE WeaveID = @WeaveID";
                SqlCommand cmd = new SqlCommand("UpdateWeavingProcess", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WeaveID", WeaveID);
                cmd.Parameters.AddWithValue("@weave_type", weave_type);
                cmd.Parameters.AddWithValue("@loomtype", loomtype);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void DeleteWeaving(int WeaveID)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                //string query = "DELETE FROM Manufacture.Weaving WHERE WeaveID = @WeaveID";
                SqlCommand cmd = new SqlCommand("DeleteWeavingProcess", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WeaveID", WeaveID);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public Weaving_process GetWeavingById(int WeaveID)
        {
            Weaving_process weaving = null;
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                //string query = "SELECT * FROM Manufacture.Weaving WHERE WeaveID = @WeaveID";
                SqlCommand cmd = new SqlCommand("GetWeavingProcessByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WeaveID", WeaveID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    weaving = new Weaving_process
                    {
                        WeaveID = Convert.ToInt32(reader["WeaveID"]),
                        weave_type = reader["weave_type"].ToString(),
                        loomType = reader["loomtype"].ToString()
                    };
                }
                conn.Close();
            }
            return weaving;
        }
        public void AddProductColor(string colorName)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Manufacture.ProductColor (ColorName) VALUES (@ColorName)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ColorName", colorName);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public List<ProductColor> GetProductColor()
        {
            List<ProductColor> colorList = new List<ProductColor>();
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Manufacture.ProductColor";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    colorList.Add(new ProductColor
                    {
                        ColorID = Convert.ToInt32(reader["ColorID"]),
                        ColorName = reader["ColorName"].ToString()
                    });
                }
                conn.Close();
            }
            return colorList;
        }
        public ProductColor GetProductColorById(int colorID)
        {
            ProductColor productcolor = null;
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Manufacture.ProductColor WHERE ColorID = @ColorID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ColorID", colorID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    productcolor = new ProductColor
                    {
                        ColorID = Convert.ToInt32(reader["ColorID"]),
                        ColorName = reader["ColorName"].ToString()
                    };
                }
                conn.Close();
            }
            return productcolor;
        }

        public void UpdateProductColor(int colorID, string colorName)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "UPDATE Manufacture.ProductColor SET ColorName = @ColorName WHERE ColorID = @ColorID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ColorID", colorID);
                cmd.Parameters.AddWithValue("@ColorName", colorName);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void DelProductColor(int colorID)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "DELETE FROM Manufacture.ProductColor WHERE ColorID = @ColorID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ColorID", colorID);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void AddFabric(string FabricName)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Manufacture.Fabrics (FabricName) VALUES (@FabricName)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FabricName", FabricName);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

        }
        public List<Fabrics> GetFabric()
        {
            List<Fabrics> fabricList = new List<Fabrics>();
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Manufacture.Fabrics";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    fabricList.Add(new Fabrics
                    {
                        FabricID = Convert.ToInt32(reader["FabricID"]),
                        FabricName = reader["FabricName"].ToString()
                    });
                }
                conn.Close();
            }
            return fabricList;
        }
        public void UpdateFabric(int FabricID, string FabricName)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "UPDATE Manufacture.Fabrics SET FabricName = @FabricName WHERE FabricID = @FabricID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FabricID", FabricID);
                cmd.Parameters.AddWithValue("@FabricName", FabricName);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

        }
        public Fabrics GetFabricById(int id)
        {
            Fabrics fabric = null;
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Manufacture.Fabrics WHERE FabricID = @FabricID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FabricID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    fabric = new Fabrics
                    {
                        FabricID = Convert.ToInt32(reader["FabricID"]),
                        FabricName = reader["FabricName"].ToString()
                    };
                }
                conn.Close();
            }
            return fabric;
        }
        public void DeleteFabric(int FabricID)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "DELETE FROM Manufacture.Fabrics WHERE FabricID = @FabricID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FabricID", FabricID);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public List<Designs> GetDesign()
        {
            List<Designs> designList = new List<Designs>();
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Manufacture.Designs";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    designList.Add(new Designs
                    {
                        DesignID = Convert.ToInt32(reader["DesignID"]),
                        DesignType = reader["DesignType"].ToString()
                    });
                }
                conn.Close();
            }
            return designList;
        }
        public void AddCategory(string CategoryName, string Description)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Supply.Categories (CategoryName, Description) VALUES (@CategoryName, @Description)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
                cmd.Parameters.AddWithValue("@Description", Description);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public List<Categories> GetCategory()
        {
            List<Categories> categoryList = new List<Categories>();
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Supply.Categories";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categoryList.Add(new Categories
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Description = reader["Description"].ToString()
                    });
                }
                conn.Close();
            }
            return categoryList;
        }
        public void UpdateCategory(int CategoryID, string CategoryName, string Description)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "UPDATE Supply.Categories SET CategoryName = @CategoryName, Description = @Description WHERE CategoryID = @CategoryID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
                cmd.Parameters.AddWithValue("@Description", Description);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void DeleteCategory(int CategoryID)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string qry = "Delete from Supply.Categories where CategoryID=@CategoryID";
                SqlCommand cmd = new SqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public Categories GetCategoryById(int CategoryID)
        {
            Categories category = null;
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Supply.Categories WHERE CategoryID = @CategoryID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    category = new Categories
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Description = reader["Description"].ToString()
                    };
                }
                conn.Close();
            }
            return category;
        }
        //public Dictionary<string, List<SelectListItem>> GetDropdownData()
        //{
        //    Dictionary<string, List<SelectListItem>> dropdownData = new Dictionary<string, List<SelectListItem>>();
        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        string query = "SELECT FabricID, FabricName FROM Manufacture.Fabrics";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        List<SelectListItem> fabricList = new List<SelectListItem>();
        //        while (reader.Read())
        //        {
        //            fabricList.Add(new SelectListItem
        //            {
        //                Value = reader["FabricID"].ToString(),
        //                Text = reader["FabricName"].ToString()
        //            });
        //        }
        //        dropdownData.Add("Fabrics", fabricList);
        //        conn.Close();
        //    }
        //    return dropdownData;
        //}
        public void AddProductAttribute(int CategoryID, int FabricID, int DesignID, int WeaveID, int ColorID)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Manufacture.ProductAttribute (CategoryID, FabricID, DesignID, WeaveID, ColorID) VALUES (@CategoryID, @FabricID, @DesignID, @WeaveID, @ColorID)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmd.Parameters.AddWithValue("@FabricID", FabricID);
                cmd.Parameters.AddWithValue("@DesignID", DesignID);
                cmd.Parameters.AddWithValue("@WeaveID", WeaveID);
                cmd.Parameters.AddWithValue("@ColorID", ColorID);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public List<ProductAttribute> GetProductAttribute()
        {
            List<ProductAttribute> productAttributeList = new List<ProductAttribute>();
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Manufacture.ProductAttribute";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    productAttributeList.Add(new ProductAttribute
                    {
                        productAttributeid = Convert.ToInt32(reader["ProductAttributeID"]),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        FabricID = Convert.ToInt32(reader["FabricID"]),
                        DesignID = Convert.ToInt32(reader["DesignID"]),
                        WeaveID = Convert.ToInt32(reader["WeaveID"]),
                        ColorID = Convert.ToInt32(reader["ColorID"])
                    });
                }
                conn.Close();
            }
            return productAttributeList;
        }
        public ProductAttribute GetProductAttributeByid(int id)
        {
            ProductAttribute productAttribute = null;
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Manufacture.ProductAttribute WHERE ProductAttributeID = @ProductAttributeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductAttributeID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    productAttribute = new ProductAttribute
                    {
                        productAttributeid = Convert.ToInt32(reader["ProductAttributeID"]),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        FabricID = Convert.ToInt32(reader["FabricID"]),
                        DesignID = Convert.ToInt32(reader["DesignID"]),
                        WeaveID = Convert.ToInt32(reader["WeaveID"]),
                        ColorID = Convert.ToInt32(reader["ColorID"])
                    };
                }
                conn.Close();
            }
            return productAttribute;
        }
        public void UpdateProductAttribute(int productAttributeid, int CategoryID, int FabricID, int DesignID, int WeaveID, int ColorID)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "UPDATE Manufacture.ProductAttribute SET CategoryID = @CategoryID, FabricID = @FabricID, DesignID = @DesignID, WeaveID = @WeaveID, ColorID = @ColorID WHERE ProductAttributeID = @ProductAttributeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductAttributeID", productAttributeid);
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmd.Parameters.AddWithValue("@FabricID", FabricID);
                cmd.Parameters.AddWithValue("@DesignID", DesignID);
                cmd.Parameters.AddWithValue("@WeaveID", WeaveID);
                cmd.Parameters.AddWithValue("@ColorID", ColorID);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void DeleteProductAttribute(int productAttributeid)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "DELETE FROM Manufacture.ProductAttribute WHERE productAttributeid = @productAttributeid";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@productAttributeid", productAttributeid);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void AddProduction(int productAttributeid, DateTime ProductionDate, string MachineUsed, int EmployeeID, int RawMaterialID, int Quantity)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Manufacture.Production (productAttributeid, ProductionDate, MachineUsed, EmployeeID, RawMaterialID, Quantity) VALUES (@productAttributeid, @ProductionDate, @MachineUsed, @EmployeeID, @RawMaterialID, @Quantity)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@productAttributeid", productAttributeid);
                cmd.Parameters.AddWithValue("@ProductionDate", ProductionDate);
                cmd.Parameters.AddWithValue("@MachineUsed", MachineUsed);
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                cmd.Parameters.AddWithValue("@RawMaterialID", RawMaterialID);
                cmd.Parameters.AddWithValue("@Quantity", Quantity);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public List<Production> GetProduction()
        {
            List<Production> productionList = new List<Production>();
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Manufacture.Production";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    productionList.Add(new Production
                    {
                        ProductionID = Convert.ToInt32(reader["ProductionID"]),
                        productattributeid = Convert.ToInt32(reader["productattributeid"]),
                        ProductionDate = Convert.ToDateTime(reader["ProductionDate"]),
                        MachineUsed = reader["MachineUsed"].ToString(),
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        RawMaterialID = Convert.ToInt32(reader["RawMaterialID"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }
                conn.Close();
            }
            return productionList;
        }
        public Production GetProductionById(int ProductionID)
        {
            Production production = null;
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Manufacture.Production WHERE ProductionID = @ProductionID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductionID", ProductionID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    production = new Production
                    {
                        ProductionID = Convert.ToInt32(reader["ProductionID"]),
                        productattributeid = Convert.ToInt32(reader["productattributeid"]),
                        ProductionDate = Convert.ToDateTime(reader["ProductionDate"]),
                        MachineUsed = reader["MachineUsed"].ToString(),
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        RawMaterialID = Convert.ToInt32(reader["RawMaterialID"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    };
                }
                conn.Close();
            }
            return production;
        }
        public void UpdateProduction(int ProductionID, int productAttributeid, DateTime ProductionDate, string MachineUsed, int EmployeeID, int RawMaterialID, int Quantity)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "UPDATE Manufacture.Production SET productAttributeid = @productAttributeid, ProductionDate = @ProductionDate, MachineUsed = @MachineUsed, EmployeeID = @EmployeeID, RawMaterialID = @RawMaterialID, Quantity = @Quantity WHERE ProductionID = @ProductionID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductionID", ProductionID);
                cmd.Parameters.AddWithValue("@productAttributeid", productAttributeid);
                cmd.Parameters.AddWithValue("@ProductionDate", ProductionDate);
                cmd.Parameters.AddWithValue("@MachineUsed", MachineUsed);
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                cmd.Parameters.AddWithValue("@RawMaterialID", RawMaterialID);
                cmd.Parameters.AddWithValue("@Quantity", Quantity);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public int AddProducts(string ProductName, int ProductionID, decimal ProductRatings, decimal Price, string Description)
        {
            int newProductId = 0;

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = @"INSERT INTO Supply.Products (ProductName, ProductionID, ProductRatings, Price, Description)
                         OUTPUT INSERTED.ProductID
                         VALUES (@ProductName, @ProductionID, @ProductRatings, @Price, @Description)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductName", ProductName);
                cmd.Parameters.AddWithValue("@ProductionID", ProductionID);
                cmd.Parameters.AddWithValue("@ProductRatings", ProductRatings);
                cmd.Parameters.AddWithValue("@Price", Price);
                cmd.Parameters.AddWithValue("@Description", Description);

                con.Open();
                newProductId = (int)cmd.ExecuteScalar();
            }

            return newProductId;
        }
        public List<ProductView> SearchProducts(string searchTerm)
        {
            List<ProductView> products = new List<ProductView>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SearchProducts", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductView pv = new ProductView
                            {
                                Product = new Products
                                {
                                    productid = Convert.ToInt32(reader["productid"]),
                                    ProductName = reader["ProductName"].ToString(),
                                    ProductRatings = reader["ProductRatings"] != DBNull.Value ? Convert.ToDecimal(reader["ProductRatings"]) : 0,
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    Description = reader["Description"].ToString()
                                },
                                ImagePath = reader["ImagePath"]?.ToString()
                            };
                            products.Add(pv);
                        }
                    }
                }
            }

            return products;
        }

        public List<ProductView> GetProductsWithImages()
        {
            List<ProductView> list = new List<ProductView>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("GetProductsWithImages", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Products product = new Products
                    {
                        productid = dr["productid"] != DBNull.Value ? Convert.ToInt32(dr["productid"]) : 0,
                        ProductName = dr["ProductName"] != DBNull.Value ? dr["ProductName"].ToString() : null,
                        ProductionID = dr["ProductionID"] != DBNull.Value ? Convert.ToInt32(dr["ProductionID"]) : 0,
                        ProductRatings = dr["ProductRatings"] != DBNull.Value ? Convert.ToDecimal(dr["ProductRatings"]) : 0,
                        Price = dr["Price"] != DBNull.Value ? Convert.ToDecimal(dr["Price"]) : 0,
                        Description = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : null
                    };

                    string imagePath = dr["ImagePath"] != DBNull.Value ? dr["ImagePath"].ToString() : null;

                    list.Add(new ProductView
                    {
                        Product = product,
                        ImagePath = imagePath
                    });
                }
            }

            return list;
        }
        public void UpdateProductImage(int productid, string newImagePath)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("UpdateProductImage", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@productid", productid);
                    cmd.Parameters.AddWithValue("@NewImagePath", newImagePath);

                    SqlParameter outputParam = new SqlParameter("@IsUpdated", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    bool isUpdated = Convert.ToBoolean(outputParam.Value);
                    if (!isUpdated)
                    {
                        throw new Exception("Product image update failed. Product may not exist.");
                    }
                }
            }
        }



        public List<Products> GetProducts()
        {
            List<Products> productList = new List<Products>();
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Supply.Products";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    productList.Add(new Products
                    {
                        productid = reader["productid"] != DBNull.Value ? Convert.ToInt32(reader["productid"]) : 0,
                        ProductName = reader["ProductName"] != DBNull.Value ? reader["ProductName"].ToString() : null,
                        ProductionID = reader["ProductionID"] != DBNull.Value ? Convert.ToInt32(reader["ProductionID"]) : 0,
                        ProductRatings = reader["ProductRatings"] != DBNull.Value ? Convert.ToDecimal(reader["ProductRatings"]) : 0,
                        Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0,
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null
                    });
                }
                conn.Close();
            }
            return productList;
        }

        public Products GetProductsbyid(int productid)
        {
            Products product = null;

            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = @"SELECT productid, ProductName, ProductionID, ProductRatings, Price, Description 
                         FROM Supply.Products 
                         WHERE productid = @productid";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@productid", productid);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Products
                            {
                                productid = reader["productid"] != DBNull.Value ? Convert.ToInt32(reader["productid"]) : 0,
                                ProductName = reader["ProductName"] != DBNull.Value ? reader["ProductName"].ToString() : null,
                                ProductionID = reader["ProductionID"] != DBNull.Value ? Convert.ToInt32(reader["ProductionID"]) : 0,
                                ProductRatings = reader["ProductRatings"] != DBNull.Value ? Convert.ToDecimal(reader["ProductRatings"]) : 0,
                                Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0,
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null
                            };
                        }
                    }
                }
            }

            return product;
        }

        public void UpdateProduct(int productid, string ProductName, int ProductionID, decimal ProductRatings, decimal Price, string Description)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UpdateProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@productid", productid);
                cmd.Parameters.AddWithValue("@ProductName", ProductName);
                cmd.Parameters.AddWithValue("@ProductionID", ProductionID);
                cmd.Parameters.AddWithValue("@ProductRatings", ProductRatings);
                cmd.Parameters.AddWithValue("@Price", Price);
                cmd.Parameters.AddWithValue("@Description", Description);

                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteProduct(int productid)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("DeleteProduct", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@productid", productid);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public List<employees> GetEmployees()
        {
            List<employees> employeeList = new List<employees>();
            SqlConnection conn = _dbHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT EmployeeID, Name FROM hrm.Employees", conn);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                employees e = new employees();
                e.EmployeeID = Convert.ToInt32(reader["EmployeeID"]);
                e.Name = reader["Name"].ToString();
                employeeList.Add(e);
            }
            reader.Close();
            conn.Close();

            return employeeList;
        }

        public int AddProductImage(int productid, string imagePath)
        {
            int newImageID = 0;

            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("InsertProductImage", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.AddWithValue("@productid", productid);
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);

                    // Output parameter
                    SqlParameter outputParam = new SqlParameter("@NewImageID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();


                    if (outputParam.Value != DBNull.Value)
                    {
                        newImageID = Convert.ToInt32(outputParam.Value);
                    }
                    else
                    {

                        throw new Exception("Invalid ProductID. Insertion failed.");
                    }
                }
            }

            return newImageID;
        }
        public string? GetImagePathByProductId(int productId)
        {
            string? imagePath = null;

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT TOP 1 ImagePath FROM Supply.ProductImage WHERE productid = @productid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@productid", productId);

                con.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    imagePath = result.ToString();
                }
            }

            return imagePath;
        }


        public List<ProductImage> GetProductImages()
        {
            List<ProductImage> productImageList = new List<ProductImage>();
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Supply.ProductImage";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    productImageList.Add(new ProductImage
                    {
                        Imageid = Convert.ToInt32(reader["Imageid"]),
                        productid = Convert.ToInt32(reader["productid"]),
                        ImagePath = reader["ImagePath"].ToString()
                    });
                }
                conn.Close();
            }
            return productImageList;
        }
        public ProductImage GetProductImageById(int ImageID)
        {
            ProductImage productImage = null;
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Supply.ProductImage WHERE ImageID = @ImageID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ImageID", ImageID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    productImage = new ProductImage
                    {
                        Imageid = Convert.ToInt32(reader["Imageid"]),
                        productid = Convert.ToInt32(reader["productid"]),
                        ImagePath = reader["ImagePath"].ToString()
                    };
                }
                conn.Close();
            }
            return productImage;
        }

    }


}