IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveOrphanDCLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveOrphanDCLeads]
GO

	


-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 28th Sep 2011
-- Description:	This proc Insert data in CRM_SaveOrphanDCLeads.
-- =============================================
CREATE PROCEDURE [dbo].[CRM_SaveOrphanDCLeads]
	
	(
	@CbdId		NUMERIC,
	@DealerId	NUMERIC,
	@DCId		NUMERIC,
	@type		NUMERIC
	
	)
	AS
	BEGIN
		INSERT INTO CRM_OrphanDCLeads
			(CBDId,DealerId,DCId,type)
		VALUES
			(@CbdId,@DealerId,@DCId,@type)	 
	END
						
							
	
	
	


