GO
CREATE PROCEDURE [dbo].[DeleteCategoryById]
	@id int
AS DELETE FROM [dbo].[Categories]
WHERE [dbo].[Categories].[CategoryID] = @id

GO
CREATE PROCEDURE [dbo].[DeleteEmployeeById]
	@id int
AS DELETE FROM [dbo].[Employees]
WHERE [dbo].[Employees].[EmployeeID] = @id

GO
CREATE PROCEDURE [dbo].[DeleteProductById]
	@id int
AS DELETE FROM [dbo].[Products]
WHERE [dbo].[Products].[ProductID] = @id

GO
CREATE PROCEDURE [dbo].[FindCategoryById]
	@id int
AS SELECT * FROM [dbo].[Categories]
WHERE [dbo].[Categories].[CategoryID] = @id

GO
CREATE PROCEDURE [dbo].[FindProductById]
	@id int
AS SELECT * FROM [dbo].[Products]
WHERE [dbo].[Products].[ProductID] = @id

GO
CREATE PROCEDURE [dbo].[GetCategories]
	@offset int,
	@limit int
AS SELECT * FROM [dbo].[Categories] as c
ORDER BY [c].[CategoryID]
OFFSET @offset ROWS
FETCH FIRST @limit ROWS ONLY

GO
CREATE PROCEDURE [dbo].[GetCategoriesByName]
	@name nvarchar(15)
AS SELECT * FROM [dbo].[Categories]
WHERE [dbo].[Categories].[CategoryName] LIKE '%'+@name+'%'

GO
CREATE PROCEDURE [dbo].[GetCategoryById]
	@id int
AS SELECT * FROM [dbo].[Categories]
WHERE [dbo].[Categories].[CategoryID] = @id

GO
CREATE PROCEDURE [dbo].[GetEmployeeById]
	@id int
AS SELECT * FROM [dbo].[Employees]
WHERE [dbo].[Employees].[EmployeeID] = @id

GO
CREATE PROCEDURE [dbo].[GetEmployees]
	@offset int,
	@limit int
AS SELECT * FROM [dbo].[Employees] as e
ORDER BY [e].[EmployeeID]
OFFSET @offset ROWS
FETCH FIRST @limit ROWS ONLY

GO
CREATE PROCEDURE [dbo].[GetProductById]
	@id int
AS SELECT * FROM [dbo].[Products]
WHERE [dbo].[Products].[ProductID] = @id

GO
CREATE PROCEDURE [dbo].[GetProducts]
	@offset int,
	@limit int
AS SELECT * FROM [dbo].[Products] as p
ORDER BY [p].[ProductID]
OFFSET @offset ROWS
FETCH FIRST @limit ROWS ONLY

GO
CREATE PROCEDURE [dbo].[GetProductsByCategory]
	@categoryId int
AS SELECT * FROM [dbo].[Products]
WHERE [dbo].[Products].[CategoryID] = @categoryId

GO
CREATE PROCEDURE [dbo].[GetProductsByName]
	@name nvarchar(40)
AS SELECT * FROM [dbo].[Products]
WHERE [dbo].[Products].[ProductName] LIKE '%'+@name+'%'

GO
CREATE PROCEDURE [dbo].[InsertCategory]
	@categoryName nvarchar(15),
	@description ntext = null,
	@picture image = null
AS INSERT INTO [dbo].[Categories] (CategoryName, Description, Picture)
VALUES (@categoryName, @description, @picture)
SELECT SCOPE_IDENTITY()

GO
CREATE PROCEDURE [dbo].[InsertEmployee]
	@lastName nvarchar(20),
	@firstName nvarchar(10),
	@title nvarchar(30) = null,
	@titleOfCourtesy nvarchar(25) = null,
	@birthDate datetime = null,
	@hireDate datetime = null,
	@address nvarchar(60) = null,
	@city nvarchar(15) = null,
	@region nvarchar(15) = null,
	@postalCode nvarchar(10) = null,
	@country nvarchar(15) = null,
	@homePhone nvarchar(24) = null,
	@extension nvarchar(4) = null,
	@image image = null,
	@notes ntext = null,
	@reportsTo int = null,
	@photoPath nvarchar(255) = null
AS INSERT INTO [dbo].[Employees]
(LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Photo, Notes, ReportsTo, PhotoPath)
VALUES (@lastName, @firstName, @title, @titleOfCourtesy, @birthDate, @hireDate, @address, @city, @region, @postalCode, @country, @homePhone, @extension, @image, @notes, @reportsTo, @photoPath)
SELECT SCOPE_IDENTITY()

GO
CREATE PROCEDURE [dbo].[InsertProduct]
	@productName nvarchar(40),
	@supplierId int = null,
	@categoryId int = null,
	@quantityPerUnit nvarchar(20) = null,
	@unitPrice money = null,
	@unitsInStock smallint = null,
	@unitsOnOrder smallint = null,
	@reorderLevel smallint = null,
	@discontinued bit
AS INSERT INTO dbo.Products
(ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued)
VALUES (@productName, @supplierId, @categoryId, @quantityPerUnit, @unitPrice, @unitsInStock, @unitsOnOrder, @reorderLevel, @discontinued)
SELECT SCOPE_IDENTITY()

GO
CREATE PROCEDURE [dbo].[UpdateCategory]
	@id int,
	@categoryName nvarchar(15),
	@description ntext = null,
	@picture image = null
AS UPDATE [dbo].[Categories]
SET CategoryName = @categoryName, Description = @description, Picture = @picture
WHERE [dbo].[Categories].[CategoryID] = @id

GO
CREATE PROCEDURE [dbo].[UpdateEmployee]
	@id int,
	@lastName nvarchar(20),
	@firstName nvarchar(10),
	@title nvarchar(30) = null,
	@titleOfCourtesy nvarchar(25) = null,
	@birthDate datetime = null,
	@hireDate datetime = null,
	@address nvarchar(60) = null,
	@city nvarchar(15) = null,
	@region nvarchar(15) = null,
	@postalCode nvarchar(10) = null,
	@country nvarchar(15) = null,
	@homePhone nvarchar(24) = null,
	@extension nvarchar(4) = null,
	@image image = null,
	@notes ntext = null,
	@reportsTo int = null,
	@photoPath nvarchar(255) = null
AS UPDATE [dbo].[Employees]
SET LastName = @lastName, FirstName = @firstName, Title = @title, TitleOfCourtesy = @titleOfCourtesy, BirthDate = @birthDate, HireDate = @hireDate, [dbo].[Employees].[Address] = @address, City = @city, Region = @region, PostalCode = @postalCode, Country = @country, HomePhone = @homePhone, Extension = @extension, Photo = @image, Notes = @notes, ReportsTo = @reportsTo, PhotoPath = @photoPath
WHERE [dbo].[Employees].[EmployeeID] = @id

GO
CREATE PROCEDURE [dbo].[UpdateProduct]
	@id int,
	@productName nvarchar(40),
	@supplierId int = null,
	@categoryId int = null,
	@quantityPerUnit nvarchar(20) = null,
	@unitPrice money = null,
	@unitsInStock smallint = null,
	@unitsOnOrder smallint = null,
	@reorderLevel smallint = null,
	@discontinued bit
AS UPDATE [dbo].[Products]
SET ProductName = @productName, SupplierID = @supplierId, CategoryID = @categoryId, QuantityPerUnit = @quantityPerUnit, UnitPrice = @unitPrice, UnitsInStock = @unitsInStock, UnitsOnOrder = @unitsOnOrder, ReorderLevel = @reorderLevel, Discontinued = @discontinued
WHERE [dbo].[Products].[ProductID] = @id