IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateInsurancePremium]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateInsurancePremium]
GO

	
-- Written By : Ashish G. Kamble on 10 Apr 2015
-- Summary : Proc to update the premium for the given lead id.
CREATE PROCEDURE [dbo].[UpdateInsurancePremium]
    @Id NUMERIC,
    @Premium DECIMAL(18,2),
	@Quotation VARCHAR(max)
 AS
   
BEGIN     	
	UPDATE INS_PremiumLeads
	SET Premium = @Premium, Quotation = @Quotation
	WHERE ID = @Id           
END

