IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_DeleteModelGroupMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_DeleteModelGroupMapping]
GO

	
-- =============================================
-- Author:		Vinay Kumar
-- Create date: 02 AUG 2013
-- Description:	This proc delete info from CRM_ADM_GroupModelMapping And Save Log Info into CRM_ADM_LogModelMapping
-- =============================================
CREATE PROCEDURE [dbo].[CRM_ADM_DeleteModelGroupMapping]
(   
    @GroupMappId VARCHAR(300),
	@userId  NUMERIC,
	@Status BIT  OUTPUT
	
)
AS
  DECLARE @GrpType INT, @ModelId INT,@idString VARCHAR(MAX)
BEGIN

	SET @idString = @GroupMappId +','	

	WHILE CHARINDEX(',', @idString) > 0 
		BEGIN
			DECLARE @tmpstr VARCHAR(100)
			 SET @tmpstr = SUBSTRING(@idString, 1, ( CHARINDEX(',', @idString) - 1 ))
				 SELECT @GrpType=CGMM.GroupType,@ModelId=CGMM.ModelId FROM CRM_ADM_GroupModelMapping AS CGMM WITH(NOLOCK) WHERE CGMM.Id IN(SELECT * FROM list_to_tbl(@tmpstr))	          
				 INSERT INTO CRM_ADM_LogModelMapping(GroupType, ModelId,DeletedBy,DeletedOn) VALUES(@GrpType,@ModelId,@userId,GETDATE())
				 DELETE FROM CRM_ADM_GroupModelMapping WHERE Id IN(SELECT * FROM list_to_tbl(@tmpstr))
			SET @idString = SUBSTRING(@idString, CHARINDEX(',', @idString) + 1, LEN(@idString))
         END
    SET  @Status = 1
END
   
