IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveComment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveComment]
GO

	-- =============================================
-- Author	:	Ajay Singh(24rd Nov 2015)
-- Description	:to Save Comment Corrosponding to Transcation  
-- EXECUTE DCRM_SaveComment 10658,NULL
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_SaveComment]
@TranscationId INT=NULL,
@Comment VARCHAR(1000) = NULL,
@Flag BIT = false
AS
	BEGIN
		IF @Flag>0
			BEGIN
				UPDATE  DCRM_PaymentTransaction  SET Comments=ISNULL(@Comment,'') WHERE TransactionId=@TranscationId
			END
		ELSE
			BEGIN
				SELECT Comments FROM DCRM_PaymentTransaction WITH(NOLOCK) WHERE TransactionId=@TranscationId
			END
	END