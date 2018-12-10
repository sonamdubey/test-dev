IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'ViwTicketFollowUps' AND
     DROP VIEW dbo.ViwTicketFollowUps
GO

	


CREATE   VIEW ViwTicketFollowUps AS
 SELECT FT.ID,FT.TicketID,FT.followedByID,SubmissionDate ,NextFollowUpDate , Comments
FROM OprFollowupTicket AS FT WHERE 
ID IN ( SELECT TOP 1 ID FROM OprFollowupTicket WHERE TicketID = FT.TicketID
 ORDER BY SubmissionDate DESC ) 




