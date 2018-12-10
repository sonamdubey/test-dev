IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Inq_GetPG_Transactions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Inq_GetPG_Transactions]
GO

	
-- PROCEDURE TO GET SCHEDULED CALLS FOR TELECALLER

CREATE PROCEDURE [dbo].[Inq_GetPG_Transactions]

	@startIndex     INT,
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@ResponceCode   INT
	
	
	
AS
	
	
BEGIN

	SET NOCOUNT ON
		DECLARE
				@upperBound INT,
				@lowerBound INT,
				@pageSize INT,
				@Responce VARCHAR(100),
				@totalData INT 
				
				IF @startIndex  < 1 SET @startIndex = 1
				SET @pageSize = 10
				
				SET @upperBound = @startIndex * @pageSize
				SET @lowerBound=(@startIndex-1)* @pageSize
				
				IF(@ResponceCode = 0)
				BEGIN
					SELECT @totalData = Count( Pg.ID)
					FROM Packages AS P, Customers AS C,PGTransactions AS Pg 
					WHERE Pg.Packageid = P.Id AND Pg.ConsumerType = 2 
						AND  Pg.ConsumerId = C.Id AND  EntryDateTime 
						BETWEEN @FromDate AND @ToDate
						AND Pg.ResponseCode =0
				  
					SELECT @totalData AS totalData --will give total count of data	
				
					 
					SELECT PP.ID, ConsumerType, CustomerName,CustomerId, ContactNo,Tel,City,City1,ProfileId,Car,Package,Amount, EntryDateTime,Paid,PackageType,PGSource,SourceId  
					FROM ( SELECT Pg.ID, ConsumerType, C.Name AS CustomerName, C.ID AS CustomerId, 
								(C.Phone1 +', '+ C.Phone2 +', '+ C.Mobile) AS ContactNo,
								C.Mobile AS Tel, Cty.Name AS City, Cty1.Name AS City1,
								'S' + Convert(Varchar, CarId) AS ProfileId, 
								(Ma.Name +' '+ Mo.Name +''+ Vr.Name) AS Car, P.Name AS Package, 
								Pg.Amount AS Amount, EntryDateTime,  
								(Select TOP 1 id From ConsumerPackageRequests Where ConsumerType = Pg.ConsumerType AND  ConsumerId = PG.ConsumerId AND isApproved = 1) AS Paid, 
								P.InqPtCategoryId AS PackageType, Pg.PGSource AS PGSource, C.SourceId AS SourceId  ,
								ROW_NUMBER() OVER(ORDER BY EntryDateTime DESC) AS RowNumber
								FROM Packages AS P, Customers AS C,  (((((((PGTransactions AS Pg 
									LEFT JOIN CustomerSellInquiries AS Csi ON Csi.Id = Pg.CarId )  
									LEFT JOIN CustomerSellInquiryCities AS Csc ON Csc.SellInquiryId = Csi.Id)  
									LEFT JOIN Cities AS Cty ON Cty.Id = Csc.CityId)  
									LEFT JOIN Cities AS Cty1 ON Cty1.Id = Csi.CityId)  
									LEFT JOIN CarVersions AS Vr ON Vr.Id = Csi.CarVersionId)  
									LEFT JOIN CarModels AS Mo ON MO.ID = Vr.CarModelId)  
									LEFT JOIN CarMakes AS Ma ON MA.ID = Mo.CarMakeId)  
							  WHERE Pg.Packageid = P.Id AND Pg.ConsumerType = 2 
								AND  Pg.ConsumerId = C.Id AND  EntryDateTime 
								BETWEEN @FromDate AND @ToDate
								AND Pg.ResponseCode =0 )AS PP
					    
					WHERE RowNumber > CONVERT(varchar(9), @lowerBound)  AND
						  RowNumber <=  CONVERT(varchar(9), @upperBound)
					ORDER BY EntryDateTime DESC
				
				END
					
				IF(@ResponceCode = 1)
				BEGIN
					
					SELECT @totalData = Count( Pg.ID)
					FROM Packages AS P, Customers AS C,PGTransactions AS Pg 
					WHERE Pg.Packageid = P.Id AND Pg.ConsumerType = 2 
						AND  Pg.ConsumerId = C.Id AND  EntryDateTime 
						BETWEEN @FromDate AND @ToDate
						AND Pg.ResponseCode IN (1,2,-1)
				  
					SELECT @totalData AS totalData --will give total count of data	
				
					 
					SELECT PP.ID, ConsumerType, CustomerName,CustomerId, ContactNo,Tel,City,City1,ProfileId,Car,Package,Amount, EntryDateTime,Paid,PackageType,PGSource,SourceId  
					FROM ( SELECT Pg.ID, ConsumerType, C.Name AS CustomerName, C.ID AS CustomerId, 
								(C.Phone1 +', '+ C.Phone2 +', '+ C.Mobile) AS ContactNo,
								C.Mobile AS Tel, Cty.Name AS City, Cty1.Name AS City1,
								'S' + Convert(Varchar, CarId) AS ProfileId, 
								(Ma.Name +' '+ Mo.Name +''+ Vr.Name) AS Car, P.Name AS Package, 
								Pg.Amount AS Amount, EntryDateTime,  
								(Select TOP 1 id From ConsumerPackageRequests Where ConsumerType = Pg.ConsumerType AND  ConsumerId = PG.ConsumerId AND isApproved = 1) AS Paid, 
								P.InqPtCategoryId AS PackageType, Pg.PGSource AS PGSource, C.SourceId AS SourceId  ,
								ROW_NUMBER() OVER(ORDER BY EntryDateTime DESC) AS RowNumber
							FROM Packages AS P, Customers AS C,  (((((((PGTransactions AS Pg 
									LEFT JOIN CustomerSellInquiries AS Csi ON Csi.Id = Pg.CarId )  
									LEFT JOIN CustomerSellInquiryCities AS Csc ON Csc.SellInquiryId = Csi.Id)  
									LEFT JOIN Cities AS Cty ON Cty.Id = Csc.CityId)  
									LEFT JOIN Cities AS Cty1 ON Cty1.Id = Csi.CityId)  
									LEFT JOIN CarVersions AS Vr ON Vr.Id = Csi.CarVersionId)  
									LEFT JOIN CarModels AS Mo ON MO.ID = Vr.CarModelId)  
									LEFT JOIN CarMakes AS Ma ON MA.ID = Mo.CarMakeId)  
							WHERE Pg.Packageid = P.Id AND Pg.ConsumerType = 2 
									AND  Pg.ConsumerId = C.Id AND  EntryDateTime 
									BETWEEN @FromDate AND @ToDate
									AND Pg.ResponseCode IN (1,2,-1) )AS PP
					WHERE RowNumber > CONVERT(varchar(9), @lowerBound)  AND
						 RowNumber <=  CONVERT(varchar(9), @upperBound)
						ORDER BY EntryDateTime DESC
				
				END
					
				
				IF(@ResponceCode = 2)
				BEGIN
					
					SELECT @totalData = Count( Pg.ID)
					FROM Packages AS P, Customers AS C,PGTransactions AS Pg 
					WHERE Pg.Packageid = P.Id AND Pg.ConsumerType = 2 
						AND  Pg.ConsumerId = C.Id AND  EntryDateTime 
						BETWEEN @FromDate AND @ToDate
						AND Pg.ResponseCode is null 
				  
					SELECT @totalData AS totalData --will give total count of data	
				
					 
					SELECT PP.ID, ConsumerType, CustomerName,CustomerId, ContactNo,Tel,City,City1,ProfileId,Car,Package,Amount, EntryDateTime,Paid,PackageType,PGSource,SourceId  
					FROM ( SELECT Pg.ID, ConsumerType, C.Name AS CustomerName, C.ID AS CustomerId, 
								(C.Phone1 +', '+ C.Phone2 +', '+ C.Mobile) AS ContactNo,
								C.Mobile AS Tel, Cty.Name AS City, Cty1.Name AS City1,
								'S' + Convert(Varchar, CarId) AS ProfileId, 
								(Ma.Name +' '+ Mo.Name +''+ Vr.Name) AS Car, P.Name AS Package, 
								Pg.Amount AS Amount, EntryDateTime,  
								(Select TOP 1 id From ConsumerPackageRequests Where ConsumerType = Pg.ConsumerType AND  ConsumerId = PG.ConsumerId AND isApproved = 1) AS Paid, 
								P.InqPtCategoryId AS PackageType, Pg.PGSource AS PGSource, C.SourceId AS SourceId  ,
								ROW_NUMBER() OVER(ORDER BY EntryDateTime DESC) AS RowNumber
							FROM Packages AS P, Customers AS C,  (((((((PGTransactions AS Pg 
									LEFT JOIN CustomerSellInquiries AS Csi ON Csi.Id = Pg.CarId )  
									LEFT JOIN CustomerSellInquiryCities AS Csc ON Csc.SellInquiryId = Csi.Id)  
									LEFT JOIN Cities AS Cty ON Cty.Id = Csc.CityId)  
									LEFT JOIN Cities AS Cty1 ON Cty1.Id = Csi.CityId)  
									LEFT JOIN CarVersions AS Vr ON Vr.Id = Csi.CarVersionId)  
									LEFT JOIN CarModels AS Mo ON MO.ID = Vr.CarModelId)  
									LEFT JOIN CarMakes AS Ma ON MA.ID = Mo.CarMakeId)  
							WHERE Pg.Packageid = P.Id AND Pg.ConsumerType = 2 
									AND  Pg.ConsumerId = C.Id AND  EntryDateTime 
									BETWEEN @FromDate AND @ToDate
									AND Pg.ResponseCode is null )AS PP
					WHERE RowNumber > CONVERT(varchar(9), @lowerBound)  AND
						 RowNumber <=  CONVERT(varchar(9), @upperBound)
						ORDER BY EntryDateTime DESC
				
				END
												  
END

