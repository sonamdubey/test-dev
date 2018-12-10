IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustomerId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustomerId]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <17/04/2013>
-- Description:	<Returns the customerid for a combination of particular email id and phoneno. and if not inserts details into the customer table and returns the corresponding customerid>
-- Modified by : Raghu on 30-12-2013 Added WITH(NOLOCK) Condition on Customers table
-- =============================================
CREATE procedure [dbo].[GetCustomerId] 
	-- Add the parameters for the stored procedure here
	@Name			VARCHAR(50),
	@CityId			NUMERIC(18,0),
	@EmailId		VARCHAR(100),
	@PhoneNo		VARCHAR(50),
	@CustomerId     INT = NULL OUTPUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @CustomerId=Id FROM Customers WITH(NOLOCK) WHERE email=@EmailId AND Mobile=@PhoneNo
	
	IF (@CustomerId IS NULL)
	BEGIN
	INSERT INTO Customers
           ([Name]
           ,[email]
           ,[CityId]
           ,[Mobile]
           )
     VALUES(@Name,@EmailId,@CityId,@PhoneNo)
     
     SET @CustomerId=SCOPE_IDENTITY()
     
     END
END







	
	









