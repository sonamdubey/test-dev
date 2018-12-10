IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertRegDealerFollowUp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertRegDealerFollowUp]
GO

	


--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Inquiry Follow Up TABLE





CREATE PROCEDURE [dbo].[InsertRegDealerFollowUp]
	@Id			NUMERIC,	-- Id. Will be -1 if Its Insertion
	@DealerId		NUMERIC,	
	@SubmissionDate	DATETIME,	
	@NextFollowUp		DATETIME,	
	@Comments		VARCHAR(500),
	@FollowedById		NUMERIC,	
	@StatusId		NUMERIC,
	@STATUS		INTEGER OUTPUT	--return value, -1 for unsuccessfull attempt, and 0 for success
	
 AS
	DECLARE @ticketId AS INTEGER
	
BEGIN
	IF @Id = -1 -- Insertion
		SET @STATUS = 1

		BEGIN

			BEGIN TRAN INSERTTICKET
	
			INSERT INTO OprDealerTicket ( DealersId , Reason , EntryDate , OpenedById , StatusID  )
			 VALUES ( @DealerId , @Comments , @SubmissionDate ,  @FollowedById , @StatusId )

			SET @ticketId = SCOPE_IDENTITY()

			INSERT INTO OprFollowupTicket ( TicketId , Comments , SubmissionDate , NextFollowupDate , FollowedById , StatusID  )
			 VALUES ( @ticketId , @Comments , @SubmissionDate , @NextFollowUp , @FollowedById , @StatusId )


			SET @STATUS = 0

			COMMIT TRAN INSERTTICKET
		END
	
					
	
END
