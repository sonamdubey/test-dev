IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveOrphanPELeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveOrphanPELeads]
GO

	

-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 28th Sep 2011
-- Description:	This proc Insert data in CRM_OrphanPELeads.
-- =============================================
CREATE PROCEDURE [dbo].[CRM_SaveOrphanPELeads]
	
	(
	@CbdId		NUMERIC,
	@MakeId		INT,
	@ModelId	INT,
	@StateId	INT,
	@CityId		INT
	
	)
	AS
	BEGIN
	
	
		INSERT INTO CRM_OrphanPELeads
			(CBDId,MakeId,ModelId,StateId,CityId)
		VALUES
			(@CbdId,@MakeId,@ModelId,@StateId,@CityId)	 
				                
							
							  
		
	END
						
							
	
	
	

