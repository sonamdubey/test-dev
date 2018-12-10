IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[list_to_tbl]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[list_to_tbl]
GO

	
CREATE FUNCTION [dbo].[list_to_tbl] (@list nvarchar(MAX))
   RETURNS @tbl TABLE (number int NOT NULL) AS
BEGIN
   DECLARE @pos        int,
           @nextpos    int,
           @valuelen   int 

   SELECT @pos = 0, @nextpos = 1 

   WHILE @nextpos > 0
   BEGIN
      SELECT @nextpos = charindex(',', @list, @pos + 1)
      SELECT @valuelen = CASE WHEN @nextpos > 0
                              THEN @nextpos
                              ELSE len(@list) + 1
                         END - @pos - 1
      INSERT @tbl (number)
         VALUES (convert(int, substring(@list, @pos + 1, @valuelen)))
      SELECT @pos = @nextpos
   END
  RETURN
END
