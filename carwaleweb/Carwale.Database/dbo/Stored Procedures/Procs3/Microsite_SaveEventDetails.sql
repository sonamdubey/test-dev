IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_SaveEventDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_SaveEventDetails]
GO

	
-- =============================================
-- Author:		Komal Manjare
-- Create date: 17 October 2016
-- Description:	save and update event details
-- =============================================
create PROCEDURE [dbo].[Microsite_SaveEventDetails] 
@Id INT,
@EventName VARCHAR(300),
@DealerId INT,
@EventDate DATETIME,
@EventId INT OUTPUT,
@AttachFileName VARCHAR(1500)=NULL

AS
BEGIN
SET NOCOUNT ON;
	DECLARE @CurrentDate DATETIME=GETDATE()
	
	IF(@Id IS NULL OR @Id=-1)
	BEGIN 
		INSERT INTO Microsite_DealerEvents(DealerId,EventName,EventDate,CreatedOn)
					VALUES(@DealerId,@EventName,@EventDate,@CurrentDate)
					SET @EventId=SCOPE_IDENTITY()
	END
    ELSE
	BEGIN
		UPDATE Microsite_DealerEvents
			SET EventName=ISNULL(@EventName,EventName),
			EventDate=ISNULL(@EventDate,EventDate),
			UpdatedOn=@CurrentDate,
			IsActive=0 -- whenever event is edited it will be deactivated
		WHERE Id=@Id
		SET @EventId=@Id
	END 
	IF(@AttachFileName IS NOT NULL AND @AttachFileName!='')
	BEGIN
		INSERT INTO Microsite_EventImages (EventId,AttachFileName,CreatedOn)	
		SELECT @EventId, CAST(@EventId as varchar)+'_'+ListMember,@CurrentDate FROM fnSplitCSVToChar(@AttachFileName)						
	END 
END
