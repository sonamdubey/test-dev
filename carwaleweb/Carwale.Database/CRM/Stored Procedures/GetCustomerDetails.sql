IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetCustomerDetails]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 8-2-2012
-- Description:	Get CRM Customer Details
-- =============================================
CREATE PROCEDURE [CRM].[GetCustomerDetails]
	-- Add the parameters for the stored procedure here
	--@CustName varchar(100),
	@email varchar(100) =NULL,
	@Mobile varchar(20) =NULL
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF (@email is not null)
    SELECT ID, Email, Name, Phone1, Mobile 
	FROM Customers WITH(NOLOCK)
	WHERE email LIKE '%'+@email+'%'
	
	IF (@@ROWCOUNT = 0)
	IF (@Mobile is not null)
    SELECT ID, Email, Name, Phone1, Mobile 
	FROM Customers WITH(NOLOCK)
	WHERE Mobile LIKE '%'+@Mobile+'%'	
	
END
