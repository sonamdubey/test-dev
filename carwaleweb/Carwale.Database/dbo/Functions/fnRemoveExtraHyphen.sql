IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fnRemoveExtraHyphen]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[fnRemoveExtraHyphen]
GO

	
CREATE FUNCTION [dbo].[fnRemoveExtraHyphen]  (@Number AS varchar(MAX))
Returns Varchar(MAX)
As
Begin
Declare @n int  -- Length of counter
Declare @old char(1)

Set @n = 1
--Begin Loop of field value
While @n <=Len (@Number)
    BEGIN
     If Substring(@Number, @n, 1) = '-' AND @old = '-'
      BEGIN
        Select @Number = Stuff( @Number , @n , 1 , '' )
      END
     Else
      BEGIN
       SET @old = Substring(@Number, @n, 1)
       Set @n = @n + 1
      END
    END
Return @number
END

