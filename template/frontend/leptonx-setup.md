# LeptonX Theme Setup (Angular)

## Install
```
npm install @volosoft/abp.ng.theme.lepton-x
```

## Import Modules
```ts
// app.module.ts
import { ThemeLeptonXModule } from '@volosoft/abp.ng.theme.lepton-x';
import { SideMenuLayoutModule } from '@volosoft/abp.ng.theme.lepton-x/layouts';

@NgModule({
  imports: [
    ThemeLeptonXModule.forRoot(),
    SideMenuLayoutModule.forRoot(),
  ],
})
export class AppModule {}
```

## Styles + RTL
```scss
// styles.scss
@import '@volosoft/abp.ng.theme.lepton-x/styles/lepton-x.min.css';
[dir='rtl'] { @import '@volosoft/abp.ng.theme.lepton-x/styles/lepton-x-rtl.min.css'; }
```

## Navigation (Policies)
- احرص على تعيين requiredPolicy وفق صلاحيات ABP.
