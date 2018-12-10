IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RemoveSpecialCharsExceptBlank]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[RemoveSpecialCharsExceptBlank]
GO

	CREATE function [dbo].[RemoveSpecialCharsExceptBlank] (@s varchar(2500)) returns varchar(2500)
begin
   if @s is null
      return null
   declare @s2 varchar(2500)
   set @s2 = ''
   declare @l int
   set @l = len(@s)
   declare @p int
   set @p = 1
   while @p <= @l begin
      declare @c int
      set @c = ascii(substring(@s, @p, 1))
      if @c between 48 and 57 or @c between 65 and 90 or @c between 97 and 122 or @c=32 or @c=45
         set @s2 = @s2 + char(@c)
      set @p = @p + 1
      end
   if len(@s2) = 0
      return null
   return @s2
   end
