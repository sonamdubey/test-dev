IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetRoadTestDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetRoadTestDetails]
GO

	
-- ================================================
-- Author:		Supriya
-- Create date: 10/31/2012
-- Description:	SP for fetching Road Test Details
-- ================================================

CREATE PROCEDURE [App].[GetRoadTestDetails]
	@BasicId Integer,
	@Priority Integer=-1
AS
BEGIN
	
	SET NOCOUNT ON;
	Exec App.GetRoadTestPageContent @BasicId,@Priority
	--Procedure to fetch contents of a roadtest page
	If @Priority=1 
	begin
		Exec App.GetRoadTestPage @BasicId
		--Procedure for fetching Road Test Details of particular RoadTest
	end
	
   
END
