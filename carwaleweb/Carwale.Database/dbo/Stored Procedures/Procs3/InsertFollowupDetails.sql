IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFollowupDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFollowupDetails]
GO

	
--this procedure is for adding the entry in the followup details table
--ID, FollowupId, FollowupDescription, FollowedById, FollowupDate, NextFollowupDate, ForServiceProvider, StatusId
CREATE PROCEDURE [InsertFollowupDetails] 
	@FollowupId			NUMERIC,
	@FollowupDescription		VARCHAR(1500),
	@FollowedById			NUMERIC,
	@FollowupDate			DATETIME,
	@NextFollowupDate		DATETIME,
	@ForServiceProvider		BIT,
	@StatusId			NUMERIC	
		
AS
	
BEGIN
	INSERT INTO FollowupDetails 
	(
		FollowupId, 		FollowupDescription, 		FollowedById, 		FollowupDate, 	
		NextFollowupDate, 	ForServiceProvider, 		StatusId
	)
	VALUES
	(
		@FollowupId, 		@FollowupDescription, 		@FollowedById, 	@FollowupDate, 	
		@NextFollowupDate, 	@ForServiceProvider, 		@StatusId
	)
	
END
