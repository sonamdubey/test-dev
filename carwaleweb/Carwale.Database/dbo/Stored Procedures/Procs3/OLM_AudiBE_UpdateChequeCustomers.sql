IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_UpdateChequeCustomers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_UpdateChequeCustomers]
GO

	
-- ======================================================
-- Author:		Supriya K.
-- Create date: 9/8/2013
-- Description:	Proc to update customer name & mobile no 
--				against customerId in customers table
-- ======================================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_UpdateChequeCustomers]
	@customerId Numeric,
	@name VARCHAR(100),
	@mobile VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE OLM_AudiBE_Customers set Name= @name, Mobile =@mobile where id=@customerId
	
END

