IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fnRemoveFirstHyphen]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[fnRemoveFirstHyphen]
GO

	
CREATE FUNCTION [dbo].[fnRemoveFirstHyphen]  (@Number AS varchar(MAX))
Returns Varchar(MAX)
As
Begin
Declare @n int  -- Length of counter
Declare @ln int  -- Length of counter
Declare @old char(1)

Set @n = 1
set @ln=Len (@Number)
--Begin Loop of field value
While @n <=Len (@Number)
    BEGIN
     If Substring(@Number, 1, 1) = '-'  
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

