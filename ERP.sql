--last Final change 26-03-25
--create database ERP_db
--use ERP_db

CREATE SCHEMA hrm
go
CREATE SCHEMA Manufacture
go
CREATE SCHEMA Supply
go
create schema crm
go
create schema sales
go
create schema delivery
go
create schema inventory
go
create schema finance

CREATE TABLE hrm.adminLogin(
adminID int Identity(1,1), --pk
adminName varchar(50) ,
email varchar(30) unique,
[password] varchar(10),
constraint pk_dept primary key(adminID)
)
CREATE TABLE hrm.FAQ(
FAQID int Identity(10,10), --pk
Questions text,
Answers text,
constraint pk_faq primary key(FAQID)
)
CREATE TABLE hrm.departments(
DepartmentID int Identity(10,10), --pk
DepartmentName varchar(50) ,
constraint pk_deptID primary key(DepartmentID)
)

CREATE TABLE hrm.roles(
role_id int Identity(1,1), --pk
role_name varchar(50),
constraint pk_role primary key(role_id)
)

CREATE TABLE hrm.employees(
EmployeeID int Identity(100,1) PRIMARY KEY, --pk
Name varchar(30),
Password varchar(10),
Email varchar(50),
PhoneNumber bigint,
RollID int references hrm.roles(role_id),
DepartmentID int,--fk(dept)
Salary dec(10,2)
)


CREATE TABLE hrm.payroll(
Payroll_ID int Identity(1,1) PRIMARY KEY, --pk
EmployeeID int , --fk(employees)
Amount dec(10,2),
PayDate date,
PayType nvarchar(20)
)



CREATE TABLE hrm.employee_attendence(
AttendanceID int Identity(1,1) PRIMARY KEY, --pk
EmployeeID int, --fk(employees)
Date date,
Status VARCHAR(20),
ShiftStart Time,
ShiftEnd Time
)

go

CREATE TABLE hrm.leave_requests(
LeaveID int Identity(1,1) PRIMARY KEY, --pk
EmployeeID int, --fk(empoyees)
LeaveType VARCHAR(50),
StartDate date,
EndDate date,
Status tinyInt
)

go

CREATE TABLE hrm.employee_training(
TrainingID int Identity(1,1) PRIMARY KEY, --pk
EmployeeID int references hrm.employees(EmployeeID), --fk(employees)
TrainingName VARCHAR(100),
TrainingDate date,
Duration int
)



--------------------
----------------------------Manufacture------------------------------------





-----weaving
create table Manufacture.Weaving_process(
WeaveID int primary key identity(1,1),
weave_type nvarchar(30),
loomtype nvarchar(25)
)
-- Product Color Table
CREATE TABLE Manufacture.ProductColor (
    ColorID INT PRIMARY KEY IDENTITY(1,1),
    ColorName VARCHAR(20) NOT NULL
)

-- Fabric Table
CREATE TABLE Manufacture.Fabrics (
    FabricID INT PRIMARY KEY IDENTITY(1,1),
    FabricName NVARCHAR(50) NOT NULL
)
-- Design Table
CREATE TABLE Manufacture.Designs (
    DesignID INT PRIMARY KEY IDENTITY(1,1),
    DesignType VARCHAR(20) NOT NULL
)
---- Purchase Orders Table
--CREATE TABLE Manufacture.PurchaseOrders (
--    PurchaseOrderID INT PRIMARY KEY IDENTITY(1,1),
--    SupplierID INT,
--    OrderDate DATE default getdate() NOT NULL,
--    TotalAmount DECIMAL(15,2) NOT NULL,
--    Status VARCHAR(50) NOT NULL
--)


CREATE TABLE Manufacture.RawMaterials (
    RawMaterialID INT PRIMARY KEY IDENTITY(1,1),
    MaterialName VARCHAR(255) NOT NULL,
    MaterialType VARCHAR(100),  -- Fabric, Thread, Dye, etc.
    UnitOfMeasure VARCHAR(50),  -- Meter, Kilogram, Piece, etc.
    Description TEXT
);
CREATE TABLE Manufacture.PurchaseOrders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    SupplierID INT,
    OrderDate DATE NOT NULL,
    DeliveryDate DATE,
    TotalAmount DECIMAL(10, 2),
    Status VARCHAR(50),  -- Pending, Delivered, Cancelled, etc.
   
);

CREATE TABLE Manufacture.PurchaseOrderDetails (
    OrderDetailID INT IDENTITY(1,1),
    OrderID INT PRIMARY KEY,
    RawMaterialID INT,
    Quantity DECIMAL(10, 2),  -- Quantity ordered
    UnitPrice DECIMAL(10, 2),  -- Price per unit
    TotalPrice DECIMAL(10, 2),  -- Total price for this material
);


CREATE TABLE Manufacture.Inventory (
    InventoryID INT PRIMARY KEY IDENTITY(1,1),
    RawMaterialID INT,
    QuantityInStock DECIMAL(10, 2),  -- Current stock quantity
    ReorderLevel DECIMAL(10, 2),  -- Minimum stock level
    LastUpdated DATE,
	handloomID int, --fk
   
);



CREATE TABLE Manufacture.ProductAttribute (
    productAttributeid INT PRIMARY KEY IDENTITY(1,1),
	CategoryID INT,
    FabricID INT,
    DesignID int,
    WeaveID int,
    ColorID INT
);

CREATE TABLE Manufacture.Production (
    ProductionID INT PRIMARY KEY IDENTITY(1,1),
	productAttributeid int,
    ProductionDate DATE NOT NULL,
    MachineUsed NVARCHAR(20) NOT NULL,
    EmployeeID INT,
	RawMaterialID int,
	Quantity int
)


-- Quality Control Table
CREATE TABLE Manufacture.QualityControl (
    QualityID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT,
    InspectionDate DATE NOT NULL,
    InspectedBy INT,
    QualityStatus VARCHAR(50) NOT NULL,
    DefectsFound TEXT
)





-- Product Manufacturing Cost Table
CREATE TABLE Manufacture.ProductManufacturingCost (
    ManufactureCostID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT,
    DirectLaborCost DECIMAL(15,2) NOT NULL,
    DirectMaterialCost DECIMAL(15,2) NOT NULL,
    OverheadCost DECIMAL(15,2) NOT NULL
)


-----------

---------------------Supply---------------------------------

-- Categories Table---
CREATE TABLE Supply.Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName VARCHAR(50) NOT NULL,
    [Description] VARCHAR(70) NOT NULL
)


-- Products Table
CREATE TABLE Supply.Products (
    productid INT PRIMARY KEY IDENTITY(1,1),
	ProductName NVARCHAR(50) NOT NULL,
    ProductionID int, --fk
    ProductRatings DECIMAL(3,1),
    Price DECIMAL(15,2) NOT NULL
);
Create Table Supply.ProductImage
(
Imageid int primary key identity(1,1),
productid int foreign key(productid)references Supply.Products(productid),
ImagePath nvarchar(max)
)


-- Supplier Table
CREATE TABLE Supply.Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    SupplierName VARCHAR(50) NOT NULL,
    SupplierContact NVARCHAR(15)  
)
-- Shipments Table
CREATE TABLE Supply.Shipments (
    ShipmentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT ,
    ShipmentDate DATE NOT NULL,
    TrackingNumber VARCHAR(100),
    ShippingMethod VARCHAR(50),
    DeliveryStatus VARCHAR(10) NOT NULL
)


/*
-- Supplier Invoices Table
CREATE TABLE Supply.SupplierInvoices (
    SupplierInvoiceID INT PRIMARY KEY IDENTITY(1,1),
    PurchaseOrderID INT,
    InvoiceDate DATE NOT NULL,
    TotalAmount DECIMAL(15,2) NOT NULL
)
*/

---CRM



create table countrys
(
CountryID int identity(1,1),
counteyName nvarchar(30),
constraint pk_countrys primary key(CountryID)
)


create table States
(
StateID int identity(1,1),
StateName nvarchar(30),
CountryID int,
constraint pk_sate primary key(StateID),
constraint fk_state_countryID foreign key(CountryID) references countrys(CountryID)
)

create table citys
(
cityID int identity(1,1),
citysName nvarchar(30),
StateID int,
constraint pk_citys primary key(cityID),
constraint fk_city_stateID foreign key(StateID) references States(StateID)
)
--Customer
create table crm.customers
(
customer_id int identity(1,1),
firstName nvarchar(50),
lastName nvarchar(50),
email nvarchar(50) not null,
phone bigint,
password nvarchar(10) not null,
create_at date default getdate(),
constraint pk_cust primary key(customer_id),
constraint chk_uni_email unique(email)
)
create table crm.Address_details(
addressID int identity(1,1) primary key,
customer_id int,
houseNo varchar(8),
street nvarchar(50),
cityID int,
StateID int,
CountryID int,
pincode nvarchar(6),
constraint fk_add_custID foreign key(customer_id) references crm.customers(customer_id),
constraint fk_add_countryID foreign key(CountryID) references countrys(CountryID),
constraint fk_add_stateID foreign key(StateID) references States(StateID),
constraint fk_add_cityID foreign key(cityID) references citys(cityID)
)

create table crm.campaign
(
campaign_id int identity(1,1),
campaign_name nvarchar(50),
campaign_type nvarchar(50),--'Email', 'Social Media', 'Ad', 'Referral'
start_date date,
end_date date,
campaignIMG nvarchar(max),
discount decimal(5,2),
status varchar(20) default 'deactive' 
constraint pk_camp primary key(campaign_id)
)

--alter table crm.campaign
--add constraint chk_type check(campaign_type in('Email', 'Social Media', 'Ad', 'Referral'))

--alter table crm.campaign
--add constraint chk_status check(status in('Planned','Active','Completed','Cancelled'))

create table crm.complaint
(
complaint_id int identity(1,1),
customer_id int,
productid int,
complaint_type nvarchar(50),
description text,
status nvarchar(10),--check(status in ('Open', 'In Progress', 'Resolved', 'Closed'))
created_at date default getdate(),
resolved_at date,
constraint pk_complaint primary key(complaint_id)
)



create table crm.wishlist
(
wishlist_id int identity(1,1),
customer_id int,
productid int,
added_at date default getdate(),
constraint pk_wh primary key(wishlist_id)
)


create table crm.feedback
(
feedback_id int identity(1,1),
customer_id int,--fk
productid int,
rating int, --check()
comments nvarchar(100),
submitted_at date default getdate(),
constraint pk_feedback primary key(feedback_id)
)
CREATE TABLE crm.cart (
    cartId INT PRIMARY KEY IDENTITY(1,1),
    customer_id INT NOT NULL,
    productid INT NOT NULL,
    Quantity INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
alter table crm.cart
add constraint fk_cart_custId foreign key(customer_id) references crm.customers(customer_id)
alter table crm.cart
add constraint fk_cart_prodId foreign key(productid) references supply.Products(productid)

--create table crm.survey
--(
--survey_id int identity(1,1),
--campaign_id int, --fk campaign
--question nvarchar(100),
--response nvarchar(100),
--submitted_at date
--)

-----sales


create table sales.orders
(
OrderID int identity(1,1),
customer_id int,
OrderDate date,
TotalAmount dec(15,2),
Status nvarchar(10),
handloomID int,
staffID int,
--paymentID int,
constraint pk_orders primary key(OrderID)
)
--added new 25-4


CREATE TABLE sales.PaymentDetails (
    Id INT IDENTITY(1,1) PRIMARY KEY,         -- Optional unique ID for the record
    OrderId NVARCHAR(100) NOT NULL,
    RazorpayOrderId NVARCHAR(100) NOT NULL,
    RazorpayPaymentId NVARCHAR(100) NOT NULL,
    RazorpaySignature NVARCHAR(200) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    PaymentDate DATETIME NOT NULL
);


--end changes 25-4
create table sales.order_item
(
itemID int identity(1,1),
OrderID int,--fk
productid int,--fk
Quantity int,
Price dec(15,2),
constraint pk_item primary key(itemID),
)


create table sales.salesReturn
(
returnID int identity(1,1),
OrderID int,
itemID int,
productid int,
customer_id int,
Quantity int,
amount dec(10,2),
description text,
--paypaymentID int,
returnDate date,
constraint pk_return primary key(returnID)
)

--create table sales.salesCommision
--(
--CommissionID int identity(1,1),
--EmployeeID int,
--SaleAmount dec(10,2),
--CommissionAmount DEC(5,2),
--CommissionDate date,
--constraint pk_commision primary key(CommissionID),
--constraint fk_commision_empID foreign key(EmployeeID) references hrm.employees(EmployeeID),
--)




---------------finance
--create schema finance




/*
---use for Tracking Only
create table sales.delivery
(
TrackingID int identity(1,1),
OrderID int,
customer_id int,
ShipmentDate date,
ShippingStatus VARCHAR(50),--process,dispatched,On the Way,delivered
constraint pk_delivery primary key(TrackingID),
constraint fk_delivery_ordID foreign key(OrderID) references sales.orders(OrderID),
constraint fk_delivery_custID foreign key(customer_id) references crm.customers(customer_id)
)

--alter table sales.deliver
--add constraint chk_shipStatus check(ShippingStatus in('InProcess','dispatched','OnTheWay','delivered'))
*/

/* last----
create table sales.voucher
(
voucherID INT identity(1,1),
OrderID INT,
DiscountAmount DECIMAL(15,2),
DiscountType VARCHAR(50),
constraint pk_voucher primary key(voucherID),
constraint fk_voucher_ordID foreign key(OrderID) references sales.orders(OrderID),
)
*/
---------------delivery------------------
create table delivery.Supplier
(
supplierId int identity(1,1),
supplierName varchar(50),
email nvarchar(50),
password nvarchar(50),
phone varchar(15),
constraint pk_supplier primary key(supplierId)
)

create table delivery.SupplierContracts
(
contractsID int identity(1,1),
supplierId int, --fk
[start_date] DATETIME,
end_date DATETIME,
constraint pk_contracts primary key(contractsID)
)

create table delivery.Shipping
(
shippingID int,
OrderID int, --fk
[address] NVARCHAR(100),
shipping_status VARCHAR(50), --(e.g., In Transit, Delivered)
constraint pk_Shipping primary key(shippingID)
)


create table delivery.Shipment_tracking
(
trackingID VARCHAR(100),
OrderID int,
supplierId int,
handloomID int,
constraint pk_ship_track primary key(trackingID)
)

----------------------------------

-------------inventory


create table inventory.Staffs
(
staffID INT identity(1,1),
FirstName nvarchar(50) not null,
LastName nvarchar(50) not null,
email nvarchar(50),
phone bigint,
constraint pk_staffID primary key(staffID)
)
/*
create table inventory.Warehouse(
wareHouseID INT identity(1,1),
LOC Varchar(30),
Status VARCHAR(50),
constraint pk_wareHouseID primary key(wareHouseID)
)

--alter table inventory.Warehouse
--add constraint chk_whStatus check(Status in('working','not working'))
*/
create table inventory.Handloom
(
handloomID INT identity(1,1),
handloomName nvarchar(50),
--StoreStockID int,--fk
LOC nvarchar(100)
constraint pk_store primary key(handloomID)
)

create table inventory.Stock
(
handloomID int,
productid INT,
Quantity INT,
InDate DATE,
OutDate DATE,
constraint pk_loomsStockID primary key(handloomID)
)


--------------finance

create table finance.paymentMethod
(
payMethodID int identity(1,1),
paymentMethod varchar(30),--
discount dec(2,2),--%
description varchar(100), --(T&C)
constraint pk_payMethod primary key(payMethodID)
)

CREATE TABLE finance.transactions (
    TransactionID INT PRIMARY KEY IDENTITY,
    TransactionType VARCHAR(50) NOT NULL, -- e.g., Sale, Refund, Payment
    Amount DECIMAL(18, 2) NOT NULL,
    TransactionDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    --customer_id INT, -- Foreign key to customers table
    OrderID INT, -- Foreign key to orders table
    payMethodID int, -- E.g., Credit Card, Cash, Bank Transfer, etc.
    PaymentStatus VARCHAR(50), -- e.g., Paid, Pending, Failed
    Channel VARCHAR(50) NOT NULL, -- 'Online' or 'Offline'
)


--payment table HERE

CREATE TABLE finance.accounts (
    AccountID INT PRIMARY KEY IDENTITY,
    AccountType VARCHAR(50), -- e.g., Assets, Liabilities, Revenue, Expenses
    AccountName VARCHAR(100), -- Name of the account (e.g., Sales Revenue, Accounts Payable)
    Balance DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    AccountCurrency VARCHAR(10), -- Currency code (e.g., USD, INR)
    IsActive BIT DEFAULT 1 -- Whether the account is active or not
);


CREATE TABLE finance.sales_revenue (
    RevenueID INT PRIMARY KEY IDENTITY,
    OrderID INT, -- Foreign key to sales orders
    SaleAmount DECIMAL(18, 2) NOT NULL,
    Channel VARCHAR(50) NOT NULL, -- 'Online' or 'Offline'
    SaleDate DATETIME DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE finance.sales_tax (
    TaxID INT PRIMARY KEY IDENTITY,
    TransactionID INT, -- Foreign key to transactions table
    TaxAmount DECIMAL(18, 2) NOT NULL,
    TaxRate DECIMAL(5, 2), -- Sales tax rate (e.g., 18% -> 18)
    Channel VARCHAR(50) NOT NULL -- 'Online' or 'Offline'
);


--CREATE TABLE finance.expenses (
--    ExpenseID INT PRIMARY KEY IDENTITY,
--    ExpenseCategory VARCHAR(50), -- e.g., Marketing, Rent, Salaries, etc.
--    ExpenseAmount DECIMAL(18, 2) NOT NULL,
--    ExpenseDate DATETIME DEFAULT CURRENT_TIMESTAMP,
--    Description TEXT, -- Additional details for the expense
--    IsPaid BIT DEFAULT 0 -- If the expense has been paid
--);

CREATE TABLE finance.receivables (
    ReceivableID INT PRIMARY KEY IDENTITY,
    customerID INT, -- Foreign key to customers table
    AmountDue DECIMAL(18, 2) NOT NULL,
    DueDate DATETIME, -- Date by which payment is due
    PaymentStatus VARCHAR(50), -- e.g., Pending, Paid, Overdue
);


CREATE TABLE finance.payables (
    PayableID INT PRIMARY KEY IDENTITY,
    VendorID INT, -- Foreign key to vendors (if applicable)
    AmountDue DECIMAL(18, 2) NOT NULL,
    DueDate DATETIME, -- Date by which the payment should be made
    PaymentStatus VARCHAR(50), -- e.g., Pending, Paid, Overdue
    Description TEXT -- Details of the payable
);


CREATE TABLE finance.revenue_forecasting (
    ForecastID INT PRIMARY KEY IDENTITY,
    ForecastDate DATE, -- The date or quarter for the forecast
    RevenueID INT, -- 'Online' or 'Offline'
    PredictedRevenue DECIMAL(18, 2) NOT NULL, -- Predicted revenue for the period
    ActualRevenue DECIMAL(18, 2), -- Actual revenue for the period (after sales are complete)
    Variance DECIMAL(18, 2) -- Difference between forecasted and actual revenue
);


CREATE TABLE finance.ledger_entries (
    LedgerEntryID INT PRIMARY KEY IDENTITY,
    AccountID INT, -- Foreign key to accounts
    TransactionID INT, -- Foreign key to transactions
    DebitAmount DECIMAL(18, 2),
    CreditAmount DECIMAL(18, 2),
    EntryDate DATETIME DEFAULT CURRENT_TIMESTAMP
);




--------------------constraint
alter table hrm.employees
add constraint fk_employee_deptID foreign key(DepartmentID) references hrm.departments(DepartmentID)
alter table hrm.payroll
add constraint fk_payroll_empID foreign key(EmployeeID) references hrm.employees(EmployeeID)
alter table hrm.employee_attendence
add constraint fk_attend_empID foreign key(EmployeeID) references hrm.employees(EmployeeID)
alter table hrm.leave_requests
add constraint fk_leave_empID foreign key(EmployeeID) references hrm.employees(EmployeeID)
alter table hrm.employee_training
add constraint fk_training_empID foreign key(EmployeeID) references hrm.employees(EmployeeID)
--

alter table Manufacture.PurchaseOrders
add constraint fk_purchaseOrd_Supplier FOREIGN KEY(SupplierID) REFERENCES Supply.Suppliers(SupplierID)
--alter table Manufacture.PurchaseOrderItems
--add constraint fk_purchaseOrdItem_pOrdID FOREIGN KEY(PurchaseOrderID) REFERENCES Manufacture.PurchaseOrders(PurchaseOrderID)
--alter table Manufacture.PurchaseOrderItems
--add constraint fk_purchaseOrdItem_MaterialID FOREIGN KEY(MaterialID) REFERENCES Manufacture.RawMaterials(MaterialID)

alter table Manufacture.PurchaseOrders
add constraint fk_ProcurementOrders FOREIGN KEY (SupplierID) REFERENCES Supply.Suppliers(SupplierID)
alter table Manufacture.PurchaseOrderDetails
add constraint fk_PurOrdersDetails_ord FOREIGN KEY (OrderID) REFERENCES Manufacture.PurchaseOrders(OrderID)
alter table Manufacture.PurchaseOrderDetails
add constraint fk_PurOrdersDetails_rm  FOREIGN KEY (RawMaterialID) REFERENCES Manufacture.RawMaterials(RawMaterialID)
alter table Manufacture.Inventory
add constraint fk_Inventory_rm FOREIGN KEY (RawMaterialID) REFERENCES Manufacture.RawMaterials(RawMaterialID)

alter table Manufacture.Production
add constraint fk_prodction_empID FOREIGN KEY(EmployeeID) REFERENCES HRM.Employees(EmployeeID)

alter table Manufacture.ProductAttribute
add constraint fk_productAtt_FabricID foreign key(FabricID) references Manufacture.Fabrics(FabricID)
alter table Manufacture.ProductAttribute
add constraint fk_productAtt_DesignID foreign key(DesignID) references Manufacture.Designs(DesignID)
alter table Manufacture.ProductAttribute
add constraint fk_productAtt_WeaveID foreign key(WeaveID) references Manufacture.Weaving_process(WeaveID)
alter table Manufacture.ProductAttribute
add constraint fk_productAtt_ColorID foreign key(ColorID) references Manufacture.ProductColor(ColorID)
alter table Manufacture.ProductAttribute
add constraint fk_productAtt_CategoryID foreign key(CategoryID) references Supply.Categories(CategoryID)



alter table Supply.Shipments
add constraint fk_Shipments_pordID foreign key(OrderID) references Manufacture.PurchaseOrders(OrderID)

alter table Manufacture.Production
add constraint fk_prodction_prodAtt foreign key(productAttributeid) references Manufacture.ProductAttribute(productAttributeid)

alter table Supply.ProductImage
add constraint fk_Img_prodID foreign key(productid) references Supply.Products(productid)
alter table Supply.Products
add constraint fk_product_productionID foreign key(ProductionID) references Manufacture.production(ProductionID)

alter table Manufacture.Production
add constraint fk_Production_rwMID foreign key(RawMaterialID) references Manufacture.RawMaterials(RawMaterialID)
alter table Manufacture.QualityControl
add constraint fk_QtyControll_prod foreign key(ProductID) references Supply.Products(ProductID)
alter table Manufacture.QualityControl
add constraint fk_QtyControll_empID foreign key(InspectedBy) REFERENCES HRM.Employees(EmployeeID)

alter table Manufacture.ProductManufacturingCost
add constraint fk_prodCost_ProductID foreign key(ProductID) references Supply.Products(ProductID)
--



--
alter table crm.complaint
add constraint fk_complaint_custID foreign key(customer_id) references crm.customers(customer_id)
alter table crm.complaint
add constraint fk_complaint_prodID foreign key(productid) references supply.products(productid)
alter table crm.complaint
add constraint chk_compStatus check(status in ('Open', 'In Progress', 'Resolved', 'Closed'))
alter table crm.wishlist
add constraint fk_wh_custID foreign key(customer_id) references crm.customers(customer_id)
alter table crm.wishlist
add constraint fk_wh_produID foreign key(productid) references supply.products(productid)
alter table crm.feedback
add constraint fk_fb_custID foreign key(customer_id) references crm.customers(customer_id)
alter table crm.feedback
add constraint fk_fb_produID foreign key(productid) references supply.products(productid)

alter table crm.feedback
add constraint chk_rating check(rating between 1 and 5)
alter table sales.orders
add constraint fk_orders_custID foreign key(customer_id) references crm.customers(customer_id)
alter table sales.orders
add constraint fk_orders_loomID foreign key(handloomID) references inventory.Handloom(handloomID)
alter table sales.orders
add constraint fk_orders_staffID foreign key(staffID) references inventory.Staffs(staffID)
--alter table sales.orders
--add constraint fk_ord_payId foreign key(paymentID) references finance.payment(paymentID)

alter table sales.orders
add constraint chk_status check(Status in('rejected','complited'))

alter table sales.order_item
add constraint fk_item_ordID foreign key(OrderID) references sales.orders(OrderID)
alter table sales.order_item
add constraint fk_item_produID foreign key(productid) references supply.products(productid)
alter table sales.salesReturn
add constraint fk_return_ordID foreign key(OrderID) references sales.orders(OrderID)
alter table sales.salesReturn
add constraint fk_return_itemID foreign key(itemID) references sales.order_item(itemID)
alter table sales.salesReturn
add constraint fk_return_produID foreign key(productid) references supply.products(productid)
alter table sales.salesReturn
add constraint fk_return_custID foreign key(customer_id) references crm.customers(customer_id)


--alter table sales.delivery
--add constraint fk_delivery_ordID foreign key(OrderID) references sales.orders(OrderID)
--alter table finance.payment
--add constraint fk_payment_ordID foreign key(OrderID) references sales.orders(OrderID)
--alter table sales.payment
--add constraint fk_payment_custID foreign key(customer_id) references crm.customers(customer_id)
--alter table finance.payment
--add constraint fk_payment_payID foreign key(payMethodID) references finance.paymentMethod(payMethodID)
alter table delivery.SupplierContracts
add constraint fk_contracts foreign key(supplierId) references delivery.Supplier(supplierId)
alter table delivery.Shipping
add constraint fk_contracts_ord foreign key(OrderID) references sales.orders(OrderID)
alter table delivery.Shipment_tracking
add constraint fk_track_ord foreign key(OrderID) references sales.orders(OrderID)
alter table delivery.Shipment_tracking
add constraint fk_track_supplier foreign key(supplierId) references delivery.Supplier(supplierId)
alter table delivery.Shipment_tracking
add constraint fk_track_handloom foreign key(handloomID) references inventory.Handloom(handloomID)


alter table inventory.Stock
add constraint fk_Stock_handloomID foreign key(handloomID) references inventory.Handloom(handloomID)
alter table inventory.Stock
add constraint fk_loomStock_produID foreign key(productid) references supply.products(productid)


--finance
--alter table finance.transactions
--add constraint fk_transactions_custID FOREIGN KEY (customer_id) REFERENCES crm.customers(customer_id)
alter table finance.transactions
add constraint fk_transactions_OrderID FOREIGN KEY (OrderID) REFERENCES sales.orders(OrderID)
alter table finance.transactions
add constraint fk_transactions_payMethodID FOREIGN KEY (payMethodID) REFERENCES finance.paymentMethod(payMethodID)
alter table finance.sales_revenue
add constraint fk_salerevenue_OrderID FOREIGN KEY (OrderID) REFERENCES sales.orders(OrderID)
alter table finance.sales_tax
add constraint fk_saleTax_transID FOREIGN KEY (TransactionID) REFERENCES finance.transactions(TransactionID)
alter table finance.receivables
add constraint fk_receivables_custID FOREIGN KEY (customerID) REFERENCES crm.customers(customer_id)
alter table finance.payables
add constraint fk_payables_venID FOREIGN KEY (VendorID) REFERENCES supply.suppliers (supplierid)
alter table finance.revenue_forecasting
add constraint fk_revForecast_revID FOREIGN KEY (RevenueID) REFERENCES finance.sales_revenue(RevenueID)
alter table finance.ledger_entries
add constraint fk_entries_accountID FOREIGN KEY (AccountID) REFERENCES finance.accounts(AccountID)
alter table finance.ledger_entries
add constraint fk_entries_transID FOREIGN KEY (TransactionID) REFERENCES finance.transactions(TransactionID)
