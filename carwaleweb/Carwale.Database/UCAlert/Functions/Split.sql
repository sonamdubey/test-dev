IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[Split]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [UCAlert].[Split]
GO

	CREATE FUNCTION [UCAlert].[Split](@String varchar(8000))       
returns @temptable TABLE (items varchar(8000))       
as       
begin       
    declare @idx int       
    declare @slice varchar(8000)
    declare @Delimiter char(1)  
    set @Delimiter=','     
      
    select @idx = 1       
        if len(@String)<1 or @String is null  return       
      
    while @idx!= 0       
    begin       
        set @idx = charindex(@Delimiter,@String)       
        if @idx!=0       
            set @slice = left(@String,@idx - 1)       
        else       
            set @slice = @String       
          
        if(len(@slice)>0)  
            insert into @temptable(Items) values(@slice)       
  
        set @String = right(@String,len(@String) - @idx)       
        if len(@String) = 0 break       
    end   
return       
end  