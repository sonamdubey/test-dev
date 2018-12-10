IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_DeleteGroupBenchMark]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_DeleteGroupBenchMark]
GO

	

-- =============================================
-- Author:		Vinay Kumar
-- Create date: 05 AUG 2013
-- Description:	This proc delete info from CRM_ADM_GroupBenchMark And Save Log Info into CRM_ADM_LogGroupBenchMark
-- =============================================
CREATE PROCEDURE [dbo].[CRM_ADM_DeleteGroupBenchMark]
(   
    @GroupBenchMarkId VARCHAR(300),
	@userId  NUMERIC,
	@Status BIT  OUTPUT
	
)
AS
  DECLARE @GrpId INT, @BenchMark NUMERIC(18,2),@idString VARCHAR(MAX)
BEGIN

	SET @idString = @GroupBenchMarkId +','	

	WHILE CHARINDEX(',', @idString) > 0 
		BEGIN
			DECLARE @tmpstr VARCHAR(100)
			 SET @tmpstr = SUBSTRING(@idString, 1, ( CHARINDEX(',', @idString) - 1 ))
				 SELECT @GrpId=CGBM.GroupId,@BenchMark=CGBM.BenchMark 
				      FROM CRM_ADM_GroupBenchMark AS CGBM WITH(NOLOCK)
				      WHERE CGBM.Id IN(SELECT * FROM list_to_tbl(@tmpstr))	          
				 INSERT INTO CRM_ADM_LogGroupBenchMark(GroupId, BenchMark,DeletedBy,DeletedOn) VALUES(@GrpId,@BenchMark,@userId,GETDATE())
				 DELETE FROM CRM_ADM_GroupBenchMark WHERE Id IN(SELECT * FROM list_to_tbl(@tmpstr))
			SET @idString = SUBSTRING(@idString, CHARINDEX(',', @idString) + 1, LEN(@idString))
         END
    SET  @Status = 1
END
