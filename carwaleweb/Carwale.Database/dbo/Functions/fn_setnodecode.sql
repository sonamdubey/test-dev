IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fn_setnodecode]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[fn_setnodecode]
GO

	
CREATE FUNCTION [dbo].[fn_setnodecode]
(
	@UserId INT
)
RETURNS VARCHAR(500)
AS
BEGIN

DECLARE @result VARCHAR(500) = '';

SELECT @result = coalesce(@result + '/', '') + convert(varchar(12),AllParentId) FROM 
(
	SELECT  
		T1.Id AS AllParentId, 
		T1.lvl AS LevelId   
		FROM  (SELECT ID,lvl,NodeCode,BranchId,IsActive FROM TC_Users WITH(NOLOCK)
         WHERE id = @UserId) T2 Join TC_Users T1 WITH(NOLOCK) ON
			(T1.NodeCode = T2.nodecode OR T2.NodeCode like T1.NodeCode+'%')
		AND T1.BranchId=T2.BranchId
 WHERE
    T1.IsActive=1
) AS a ORDER BY LevelId

SET @result = @result + '/';

RETURN @result;

END

