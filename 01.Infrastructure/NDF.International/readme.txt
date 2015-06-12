

NDF（Net Dirk Framework） .NET 国际化资源基础库组件（NDF.International.dll）发布说明

一、版本信息和源码：
    1、版本信息
        v1.01 beta（2015-05-19），基于 .NET4.5 开发，但源码支持在 .NET4.0 环境下编译。

    2、开放源码地址
        https://github.com/cjw0511/NDF.Infrastructure
        关于该组件源码位于文件夹：src/NDF.International 文件夹中。

    3、该组件源码采用 GPL 协议进行共享；


二、功能概述：
    1、对中文字符（包括简体中文和繁体中文）的拼音处理支持：
        a、获取某个中文字符的拼音，支持多音字和声调处理；
        b、获取某个拼音字符串对应的所有中文字符；
        c、判断某个中文字符是否具有某个拼音读音和声调；
        d、判断某个拼音字符串是否是合法拼音格式；

    2、对中文字符（包括简体中文和繁体中文）的笔画数处理支持：
        a、获取某个中文字符的笔画数；
        b、获取具有某个笔画数的所有中文字符；
        c、比较中文字符之间的笔画数差异；
        d、判断某个数值是否是合法的中文字符笔画数；

    3、支持中文字符简体和繁体之间的互相转换；

    4、支持将数值类型值（包括 double、float、int、uint、long、ulong、short、ushort、sbyte、byte 和 decimal）转换为简体中文或繁体中文格式的字符串；


三、使用说明：
    直接在项目中引入编译后的 NDF.International.dll 库文件即可。
    主要的 API 调用类型为：
    1、NDF.International.Chinese.ChineseChar，用于中文字符的拼音、笔画、简繁体转换处理；
    2、NDF.International.Chinese.ChineseWord，用于中文词语的拼音、笔画、简繁体转换处理；
    3、NDF.International.Chinese.PinYins.PinYin，用于汉语拼音的处理和校验；
    4、NDF.International.Chinese.Conversion.ChineseConverter，用于中文字符串的简繁体转换处理；
    5、NDF.International.Chinese.Conversion.NumericConverter，用于将数值转换成简体/繁体中文格式的字符串处理；
    6、NDF.International.Formatting.EastAsiaNumericFormatter，用于将数值转换成东亚地区语言格式的字符串处理；
    关于以上 API 详情请参阅代码中的 API 注释和说明。


四、其他说明：
    1、关于中文字符拼音处理功能，所有的拼音字库信息取自 Microsoft Visual Studio International Pack 1.0 中的
        Simplified Chinese Pin-Yin Conversion Library（ChnCharInfo.dll，简体中文拼音转换类库），但是没有使用该类库的 API 和源码，而是基于该拼音字库另行封装；
        因算法和基础库数据结构的调整和优化，在性能上，NDF.International.dll 库中的几乎所有 API 均比 ChnCharInfo.dll 中的同类型 API 快 10 - 100 倍。

    2、本组件中部分源码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的：
        a、East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库）
        b、Traditional Chinese to Simplified Chinese Conversion Library（ChineseConverter.dll 中文繁简转换类库）
        具体请参阅相关类文件头注释。
        关于这两个库文件的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251

