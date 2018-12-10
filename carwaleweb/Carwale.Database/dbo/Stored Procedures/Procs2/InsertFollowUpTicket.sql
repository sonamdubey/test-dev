IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFollowUpTicket]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFollowUpTicket]
GO
	


--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Inquiry Follow Up TABLE





CREATE PROCEDURE [dbo].[InsertFollowUpTicket]
	@Id			NUMERIC,	-- Id. Will be -1 if Its Insertion
	@TicketId		NUMERIC,	
	@SubmissionDate	DATETIME,	
	@NextFollowUp		DATETIME,	
	@Comments		VARCHAR(500),
	@FollowedById		NUMERIC,	
	@StatusId		NUMERIC,
	@STATUS		INTEGER OUTPUT	--return value, -1 for unsuccessfull attempt, and 0 for success
	
 AS
	
BEGIN
	IF @Id = -1 -- Insertion
		SET @STATUS = 1

		BEGIN
	
			INSERT INTO OPRFOLLOWUPTICKET ( TicketId , Comments , SubmissionDate , NextFollowUpDate , FollowedById , StatusID  )
			 VALUES ( @TicketId , @Comments , @SubmissionDate , @NextFollowUp , @FollowedById , @StatusId )
			SET @STATUS = 0
		END
	
					
	
END
