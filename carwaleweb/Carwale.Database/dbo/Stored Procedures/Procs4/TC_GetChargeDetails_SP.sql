IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetChargeDetails_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetChargeDetails_SP]
GO

	-- =============================================
-- Modified by: Surendra Chouksey
-- Create date: 17-09-2011
-- Description:	Geting diffrent and all type of charges and already paid charges
-- =============================================
-- =============================================
-- Author:		Binumon George
-- Create date: 25-08-2011
-- Description:	Geting diffrent and all type of charges and already paid charges
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetChargeDetails_SP] 
	-- Add the parameters for the stored procedure here
	@TC_CarBooking_Id INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	--This for Geting all type of charges
	SELECT TC_PaymentVariables_Id, VariableName from TC_PaymentVariables WHERE IsActive=1 ORDER BY VariableName ASC		
	--This for geting charges already paid.
	SELECT Variable.VariableName,Include.TC_PaymentOtherCharges_Id, Include.Amount,Include.Comments
		From TC_PaymentVariables Variable INNER JOIN TC_PaymentOtherCharges	Include
		ON	Variable.TC_PaymentVariables_Id = Include.TC_PaymentVariables_Id
		WHERE 	Include.TC_CarBooking_Id=@TC_CarBooking_Id
END

