IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustomerInfoByCustomerID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustomerInfoByCustomerID]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 07/05/2013
-- Description:	Get Customer Info By CustomerID
-- =============================================	
/*
	declare @CustomerID BIGINT = 1733
	declare @CustName varchar(100)
	declare @CustCity varchar(100)
	declare @CustState varchar(100)
	declare @CustEmail varchar(100)
	declare @CustMobile varchar(100)
	exec GetCustomerInfoByCustomerID @CustomerID,@CustName out,@CustCity out,@CustState out ,@CustEmail out,@CustMobile out
	SELECT @CustomerID,@CustName,@CustState,@CustCity,@CustMobile,@CustEmail
*/
CREATE PROCEDURE [dbo].[GetCustomerInfoByCustomerID]
	@CustomerID VARCHAR(100),
	@CustName VARCHAR(100) OUT,
	@CustCity VARCHAR(50) OUT,
	@CustState VARCHAR(30) OUT,
	@CustEmail VARCHAR(100) OUT,
	@CustMobile VARCHAR(20) OUT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT @CustName = NAME,
		@CustEmail = email,
		@CustState = (
			SELECT NAME
			FROM States S WITH (NOLOCK)
			WHERE S.ID = C.StateId
			),
		@CustCity = (
			SELECT NAME
			FROM Cities CI WITH (NOLOCK)
			WHERE CI.ID = C.CityId
			),
		@CustMobile = Mobile
	FROM Customers C WITH (NOLOCK)
	WHERE ID = @CustomerID

	--SELECT @CustomerID,@CustName,@CustState,@CustCity,@CustMobile,@CustEmail
END
