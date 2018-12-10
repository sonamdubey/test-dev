IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_PotentiallyLostCaseFetch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_PotentiallyLostCaseFetch]
GO

	
-- =============================================
-- Author:		AMIT KUMAR
-- Create date: 5th Nov 2012
-- Description:	To fetch the data of potentially lost case. This procedure is used in RMPanle followupdetails and OPR cardetails
-- =============================================
CREATE PROCEDURE [dbo].[CRM_PotentiallyLostCaseFetch] 
@CbdId			NUMERIC(18,0)
AS
BEGIN
	SELECT Comment,IsActionTaken, Make, Model FROM CRM_PotentiallyLostCase WHERE CBDId = @CbdId AND IsActionTaken = '0'
END

