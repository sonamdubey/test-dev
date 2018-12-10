IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_NewCarPurchaseOtherCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_NewCarPurchaseOtherCars]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 20-Feb-2014
-- Description:	Get the other interested cars details for this customer
--Table : NewCarPurchaseInquiries, vwMMV
-- =============================================
CREATE PROCEDURE [dbo].[TC_NewCarPurchaseOtherCars]
--@CustomerId BIGINT,
@CWCustomerId BIGINT,
@ModelId INT,
@FromDate DATETIME,
@ToDate DATETIME
AS
BEGIN
DECLARE 
@NoOfDayOldCustomer INT = 30
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

   SELECT TOP 10 (NCPL.CarMake +' '+ NCPL.CarModel +' ' +  NCPL.CarVersion) as Model, CONVERT(DATE,NCPL.ReqDate,110) as ReqDate FROM TC_NewCarPurchaseLead AS NCPL WITH(NOLOCK)
   WHERE NCPL.CWCustomerId =  @CWCustomerId AND NCPL.ModelId != @ModelId AND NCPL.ReqDate >= @FromDate AND NCPL.ReqDate <= @ToDate
END
