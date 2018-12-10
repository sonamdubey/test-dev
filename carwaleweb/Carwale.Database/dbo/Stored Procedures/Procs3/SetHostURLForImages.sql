IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SetHostURLForImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SetHostURLForImages]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 10-11-2011
-- Description:	Set HostURL and Replicate Images For redundancy
-- SetHostURLForImages 'Con_EditCms_Author$2,5,6$' 
-- Modified By: Vikas -- Purpose: Added "LiveListings" condition in Dynamic Sql generation
-- Modified On: 29/10/2012
-- =============================================
CREATE PROCEDURE [dbo].[SetHostURLForImages]
(@TableValues  VARCHAR(MAX))
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @SQLString VARCHAR(MAX)
	DECLARE @Pos INT
	DECLARE @NextPos INT
	DECLARE @String VARCHAR(MAX)
	DECLARE @Delimiter CHAR(1)
	declare @sql VARCHAR(MAX)
	declare @hosturl varchar(100)

	SET @sql=''
	set @hosturl='''img.carwale.com'''
	SET @String =@TableValues
	SET @Delimiter = '$'
	SET @String = @String + @Delimiter
	SET @Pos = charindex(@Delimiter,@String)
	SET @SQLString = substring(@String,1,@Pos - 1)
	if @SQLString in ('Con_EditCms_Author')
	begin
	set @sql=@sql+'Update '+@SQLString +' set IsReplicated=1, Hosturl = '+@hosturl+' where Authorid in ('
	end
	else if @SQLString in ('TC_BugFeedback')
	begin
	set @sql=@sql+'Update '+@SQLString +' set IsReplicated=1, Hosturl = '+@hosturl+' where TC_Bug_Id in ('
	end
	else if @SQLString in ('LiveListings')
	begin
	set @sql=@sql+'Update '+@SQLString +' set IsReplicated=1, Hosturl = '+@hosturl+' where Inquiryid in ('
	end
	else
	begin
	set @sql=@sql+'Update '+@SQLString +' set IsReplicated=1, Hosturl = '+@hosturl+' where Id in ('
	end
	
	SET @pos=1

	WHILE (@pos <> 0)
	BEGIN
	SET @SQLString = substring(@String,1,@Pos - 1)
	if @Pos>1
	set @sql=@sql+@SQLString +')'
	SET @Pos = charindex(@Delimiter,@String)
	SET @String = substring(@String,@pos+1,len(@String))
	SET @pos = charindex(@Delimiter,@String)
	END
	--select @sql -- Comment this line when it will go in production
	EXEC(@sql)
END
