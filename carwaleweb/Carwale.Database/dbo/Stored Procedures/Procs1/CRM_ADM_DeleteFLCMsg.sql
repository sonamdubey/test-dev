IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_DeleteFLCMsg]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_DeleteFLCMsg]
GO

	
-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 25 May 2013
-- Description : Store Deleting history In CRM_FLCMessageLog 
-- =============================================

CREATE PROCEDURE [dbo].[CRM_ADM_DeleteFLCMsg]
    (
	@ID				VARCHAR(500),
	@UpdatedBy		NUMERIC = NULL,
	@UpdatedOn		DATETIME,
	@Status			BIT OUTPUT
	)
 AS
   BEGIN
         DECLARE @Msg  VARCHAR(500)
         SET @Msg ='DELETED'	
         --deleting row from CRM_FLCMessage acording to selected id
	      DELETE FROM CRM_FLCMessage WHERE Id IN(SELECT * FROM list_to_tbl(@ID))
					
			       DECLARE @idString VARCHAR(MAX)
					SET @idString = @ID	+','

					WHILE CHARINDEX(',', @idString) > 0 
						BEGIN
							DECLARE @tmpstr VARCHAR(50)
							 SET @tmpstr = SUBSTRING(@idString, 1, ( CHARINDEX(',', @idString) - 1 ))
							 
						   --Insert Message- "DELETE" in CRM_FLCMessageLog according to MessageId
							INSERT  INTO CRM_FLCMessageLog(FLCMessageId,UpdatedBy,UpdatedOn,Message)  VALUES  ( @tmpstr,@UpdatedBy,@UpdatedOn,@Msg)
							
							SET @idString = SUBSTRING(@idString, CHARINDEX(',', @idString) + 1, LEN(@idString))
						 END
        SET  @Status = 1
            
    END

