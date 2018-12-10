IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[AC].[RemoveAddSpecialCharacters]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [AC].[RemoveAddSpecialCharacters]
GO

	
CREATE Function [ac].[RemoveAddSpecialCharacters](@Temp VarChar(1000))
Returns VarChar(1000)
AS
Begin
    Declare @KeepValues as varchar(50)
    Set @KeepValues = '%[^a-z0-9|:]%'
	--SET @Temp = replace(@Temp,'-',' ')
    While PatIndex(@KeepValues, @Temp) > 0
        Set @Temp = Stuff(@Temp, PatIndex(@KeepValues, @Temp), 1, '')

    Return lower(@Temp)
End


