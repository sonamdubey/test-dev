IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Support_SaveTicketHistory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Support_SaveTicketHistory]
GO

	
-- =============================================
-- Author:		<Komal Manjare>
-- Create date: <16-sept-2016>
-- Description:	<save ticket data and images>
-- Modified By :Komal Manjare on 5-10-2016 handle null condition for ExpecClosingDate
-- =============================================
CREATE PROCEDURE [dbo].[Support_SaveTicketHistory]  
@TicketId INT ,
@Status SMALLINT ,
@Comment VARCHAR(100)=NULL,
@CreatedBy INT,
@Rating INT=NULL,
@AttachFileName VARCHAR(100)=NULL,
@ExpectedClosureDate DATETIME= NULL
AS
BEGIN

	DECLARE @TicketLogId INT
	INSERT INTO Support_TicketLog (TicketId,CreatedOn,Status,Comment,CreatedBy)
                         VALUES(@TicketId,GETDATE(),@Status,@Comment,@CreatedBy) 
	SET @TicketLogId=SCOPE_IDENTITY()
	IF @AttachFileName IS NOT NULL AND @AttachFileName != ''
	BEGIN
		-- insert image data in Support_AttachedFileDetails
		EXEC [dbo].[Support_SaveTicketImages] @AttachFileName,@CreatedBy,@TicketLogId
	END
	UPDATE Support_Tickets 
	SET Status = @Status,LastActionDate= GETDATE(),
	ExpecClosingDate=ISNULL(@ExpectedClosureDate,ExpecClosingDate) WHERE Id=@TicketId -- Komal Manjare on 5-10-2016 

    IF(@Status=3)
    UPDATE Support_Tickets 
	SET ClosingDate = GETDATE(),Rating=@Rating
	WHERE Id=@TicketId 
		           
END
