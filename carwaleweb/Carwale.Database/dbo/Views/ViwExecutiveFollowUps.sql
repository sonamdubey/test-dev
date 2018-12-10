IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'ViwExecutiveFollowUps' AND
     DROP VIEW dbo.ViwExecutiveFollowUps
GO

	
CREATE VIEW ViwExecutiveFollowUps AS SELECT NDF.ID,NDF.DealersID,NDF.followedByID,SubmissionDate FROM NewDealersFollowup AS NDF WHERE ID IN ( SELECT TOP 1 ID FROM NewDealersFollowup WHERE DealersID = NDF.DealersID ORDER BY SubmissionDate DESC ) 
