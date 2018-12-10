IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Support_SaveTicketImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Support_SaveTicketImages]
GO

	-- =============================================
-- Author:		Komal Manjare 
-- Create date: 28 Sept 2016 
-- Description:	Save image data for support history 
-- =============================================
CREATE PROCEDURE [dbo].[Support_SaveTicketImages]
@AttachFileName VARCHAR(MAX),
@UploadedBy INT,
@TicketLogId Int
AS
BEGIN
	DECLARE @TempTicketImages TABLE(RowId INT IDENTITY(1,1), TicketFileName VARCHAR(500))
	DECLARE @TotalFiles INT , @RowCount INT , @FileName VARCHAR(500)
				
			INSERT INTO @TempTicketImages SELECT * FROM fnSplitCSVToChar(@AttachFileName)
			SET @TotalFiles = @@ROWCOUNT				
		
		IF(@TicketLogId>0)
			BEGIN
					SET @RowCount = 1
				
						WHILE(@RowCount <= @TotalFiles)
							BEGIN 
					
								SELECT @FileName = TicketFileName FROM @TempTicketImages WHERE RowId = @RowCount
								SET @FileName = CAST(@TicketLogId as varchar)+'_'+@FileName 
								INSERT INTO Support_AttachedFileDetails
											(
												TicketLogId,
												AttachedFileName,										
												UploadedOn,
												UploadedBy
											)
											VALUES
											(
												@TicketLogId,
												@FileName,
												GETDATE(),
												@UploadedBy
											)
								SET @RowCount += 1
							END
					END	
	END

