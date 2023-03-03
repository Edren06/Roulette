CREATE PROC pr_GetOrderSummary

	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL,	
	@EmployeeId INT = NULL,
	@CustomerId INT = NULL	

AS

BEGIN
	SELECT
		x.EmployeeName,
		x.OrderDate,
		x.CustomerCompanyName,
		x.ShipperCompanyName,
		MAX(x.NumberOfOrders) AS NumberOfOrders,
		MAX(x.OrderTotal) AS OrderTotal,
		MAX(x.NumberOfProducts) AS NumberOfProducts,
		MAX(x.Freight) AS FreightAmount
	FROM
	(
		SELECT
			RTRIM(LTRIM(ISNULL(emp.Title, '') + ' ' + ISNULL(emp.FirstName, '') + ' ' + ISNULL(emp.LastName, ''))) AS EmployeeName,
			header.OrderDate,
			ship.Name AS ShipperCompanyName,
			company.AccountNumber AS CustomerCompanyName, --todo
			detail.NumberOfOrders,
			detail.OrderTotal,
			detail.NumberOfProducts,
			header.Freight
		FROM Sales.SalesOrderHeader header
		LEFT JOIN HumanResources.vEmployee emp ON header.SalesPersonID = emp.BusinessEntityID
		LEFT JOIN Sales.Customer company ON header.CustomerID = company.CustomerID
		LEFT JOIN Purchasing.ShipMethod ship ON header.ShipMethodID = ship.ShipMethodID
		LEFT JOIN
		(
			SELECT
				SalesOrderID,
				COUNT(*) AS NumberOfOrders,
				SUM(LineTotal) AS OrderTotal,
				COUNT(DISTINCT ProductID) AS NumberOfProducts
			FROM Sales.SalesOrderDetail
			GROUP BY SalesOrderID
		) detail ON detail.SalesOrderID = header.SalesOrderID
	
		WHERE
			ISNULL(@EmployeeId, header.SalesPersonID) = header.SalesPersonID AND
			ISNULL(@CustomerId, header.CustomerID) = header.CustomerID AND
			(
				(@StartDate IS NULL OR @EndDate IS NULL) OR
				(
					header.OrderDate >= @StartDate AND header.OrderDate <= @EndDate
				)
			)
	) x

	GROUP BY 
		x.EmployeeName,
		x.OrderDate,
		x.CustomerCompanyName,
		x.ShipperCompanyName
END


	