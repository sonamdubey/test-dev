IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_UpdateDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_UpdateDealer]
GO

	
-- =============================================
-- Author:		Vaibhav Kale
-- Create date: 21-Feb-2012
-- Description:	Updating a Dealer of FB_SkodaDealers
-- =============================================
CREATE PROCEDURE [dbo].[OLM_UpdateDealer]
	-- Add the parameters for the stored procedure here
	@Id				NUMERIC,
	@DealerName		VARCHAR(100),
	@DealerCode		VARCHAR(50),
	@Message		VARCHAR(100) = 'SOME ERROR' OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		DECLARE @NumberRecords INT = 0
		
	SELECT Id FROM FB_SkodaDealers WITH (NOLOCK) 
		WHERE DealerCode = @DealerCode AND Id <> @Id
	SET @NumberRecords = @@ROWCOUNT
			
	IF(@NumberRecords = 0)--If no record exixt for given DealerCode
	BEGIN
		UPDATE FB_SkodaDealers 
			SET DealerName = @DealerName,
				DealerCode = @DealerCode
			WHERE Id = @Id
		SET @Message = 'UPDATE DEALER SUCCESSFULL'
	END
	
	ELSE
		BEGIN
			SET @Message = 'UPDATE DEALER FAILED: DEALER CODE ALREADY EXIST'
		END
END

